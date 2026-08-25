using UAMS.Application.DTOs.Departments.Requests;
using UAMS.Application.DTOs.Departments.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Departments;

namespace UAMS.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DepartmentService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }


    // ================================================================
    // Create Department
    // ================================================================

    public async Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        var existingByCode =
            await _unitOfWork.Departments
                .FindAsync(
                    department =>
                        department.Code.ToLower() == code.ToLower(),
                    cancellationToken);

        if (existingByCode.Count > 0)
        {
            throw new InvalidOperationException(
                "A department with the same code already exists.");
        }

        var existingByName =
            await _unitOfWork.Departments
                .GetByNameAsync(
                    name,
                    cancellationToken);

        if (existingByName is not null)
        {
            throw new InvalidOperationException(
                "A department with the same name already exists.");
        }


        // Validate Department Head when supplied
        if (request.DepartmentHeadId.HasValue)
        {
            var departmentHead =
                await _unitOfWork.Users.GetByIdAsync(
                    request.DepartmentHeadId.Value,
                    cancellationToken);

            if (departmentHead is null)
            {
                throw new KeyNotFoundException(
                    "Department head was not found.");
            }

            if (!departmentHead.IsActive)
            {
                throw new InvalidOperationException(
                    "The selected department head is inactive.");
            }
        }


        var department = Department.Create(
            code,
            name,
            request.Description,
            request.OfficeLocation,
            request.EstablishedDate,
            request.DepartmentHeadId);


        await _unitOfWork.Departments.AddAsync(
            department,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(department);
    }


    // ================================================================
    // Get Department By ID
    // ================================================================

    public async Task<DepartmentResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                id,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "Department was not found.");
        }

        return MapToResponse(department);
    }


    // ================================================================
    // Get Department Details
    // ================================================================

    public async Task<DepartmentDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                id,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "Department was not found.");
        }

        return new DepartmentDetailResponseDto
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            OfficeLocation = department.OfficeLocation,
            EstablishedDate = department.EstablishedDate,
            DepartmentHeadId = department.DepartmentHeadId,
            DepartmentHeadName = department.DepartmentHead?.FullName,
            IsActive = department.IsActive,
            UserCount = department.Users.Count,
            AssetCount = department.Assets.Count,
            AssetRequestCount = department.AssetRequests.Count
        };
    }


    // ================================================================
    // Get Departments
    // ================================================================

    public async Task<DepartmentListResponseDto> GetAllAsync(
        DepartmentFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var departments =
            await _unitOfWork.Departments.GetAllAsync(
                cancellationToken);

        IEnumerable<Department> query = departments;


        // ============================================================
        // Search
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(department =>
                department.Code.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                department.Name.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                (
                    department.Description != null
                    &&
                    department.Description.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                ));
        }


        // ============================================================
        // Code Filter
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var code = request.Code.Trim();

            query = query.Where(department =>
                department.Code.Equals(
                    code,
                    StringComparison.OrdinalIgnoreCase));
        }


        // ============================================================
        // Name Filter
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            query = query.Where(department =>
                department.Name.Contains(
                    name,
                    StringComparison.OrdinalIgnoreCase));
        }


        // ============================================================
        // Department Head Filter
        // ============================================================

        if (request.DepartmentHeadId.HasValue)
        {
            query = query.Where(department =>
                department.DepartmentHeadId ==
                request.DepartmentHeadId.Value);
        }


        // ============================================================
        // Active Status Filter
        // ============================================================

        if (request.IsActive.HasValue)
        {
            query = query.Where(department =>
                department.IsActive ==
                request.IsActive.Value);
        }


        // ============================================================
        // Established Date Filter
        // ============================================================

        if (request.EstablishedFrom.HasValue)
        {
            query = query.Where(department =>
                department.EstablishedDate.HasValue
                &&
                department.EstablishedDate.Value >=
                request.EstablishedFrom.Value);
        }

        if (request.EstablishedTo.HasValue)
        {
            query = query.Where(department =>
                department.EstablishedDate.HasValue
                &&
                department.EstablishedDate.Value <=
                request.EstablishedTo.Value);
        }


        // ============================================================
        // Sorting
        // ============================================================

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "code" =>
                request.SortDescending
                    ? query.OrderByDescending(x => x.Code)
                    : query.OrderBy(x => x.Code),

            "name" =>
                request.SortDescending
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),

            "establisheddate" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        x => x.EstablishedDate)
                    : query.OrderBy(
                        x => x.EstablishedDate),

            _ =>
                request.SortDescending
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name)
        };


        // ============================================================
        // Pagination
        // ============================================================

        var totalCount = query.Count();

        var pageNumber = request.PageNumber < 1
            ? 1
            : request.PageNumber;

        var pageSize = request.PageSize < 1
            ? 20
            : Math.Min(request.PageSize, 100);

        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount / (double)pageSize);

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();

        return new DepartmentListResponseDto
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }


    // ================================================================
    // Update Department
    // ================================================================

    public async Task<DepartmentResponseDto> UpdateAsync(
        Guid id,
        UpdateDepartmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                id,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "Department was not found.");
        }


        // ============================================================
        // Duplicate Code Check
        // ============================================================

        var code = request.Code.Trim();

        var existingByCode =
            await _unitOfWork.Departments.FindAsync(
                item =>
                    item.Id != id
                    &&
                    item.Code.ToLower() ==
                    code.ToLower(),
                cancellationToken);

        if (existingByCode.Count > 0)
        {
            throw new InvalidOperationException(
                "A department with the same code already exists.");
        }


        // ============================================================
        // Duplicate Name Check
        // ============================================================

        var existingByName =
            await _unitOfWork.Departments.GetByNameAsync(
                request.Name.Trim(),
                cancellationToken);

        if (existingByName is not null &&
            existingByName.Id != id)
        {
            throw new InvalidOperationException(
                "A department with the same name already exists.");
        }


        // ============================================================
        // Validate Department Head
        // ============================================================

        if (request.DepartmentHeadId.HasValue)
        {
            var departmentHead =
                await _unitOfWork.Users.GetByIdAsync(
                    request.DepartmentHeadId.Value,
                    cancellationToken);

            if (departmentHead is null)
            {
                throw new KeyNotFoundException(
                    "Department head was not found.");
            }

            if (!departmentHead.IsActive)
            {
                throw new InvalidOperationException(
                    "The selected department head is inactive.");
            }
        }


        department.Update(
            code,
            request.Name,
            request.Description,
            request.OfficeLocation,
            request.EstablishedDate,
            request.DepartmentHeadId);

        _unitOfWork.Departments.Update(department);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(department);
    }


    // ================================================================
    // Activate Department
    // ================================================================

    public async Task<DepartmentResponseDto> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                id,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "Department was not found.");
        }

        if (!department.IsActive)
        {
            department.Activate();

            _unitOfWork.Departments.Update(
                department);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        return MapToResponse(department);
    }


    // ================================================================
    // Deactivate Department
    // ================================================================

    public async Task<DepartmentResponseDto> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(id));
        }

        var department =
            await _unitOfWork.Departments.GetByIdAsync(
                id,
                cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                "Department was not found.");
        }

        if (department.IsActive)
        {
            department.Deactivate();

            _unitOfWork.Departments.Update(
                department);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        return MapToResponse(department);
    }


    // ================================================================
    // Get Active Departments
    // ================================================================

    public async Task<IReadOnlyList<DepartmentResponseDto>>
        GetActiveDepartmentsAsync(
            CancellationToken cancellationToken = default)
    {
        var departments =
            await _unitOfWork.Departments
                .GetActiveDepartmentsAsync(
                    cancellationToken);

        return departments
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Inactive Departments
    // ================================================================

    public async Task<IReadOnlyList<DepartmentResponseDto>>
        GetInactiveDepartmentsAsync(
            CancellationToken cancellationToken = default)
    {
        var departments =
            await _unitOfWork.Departments
                .GetInactiveDepartmentsAsync(
                    cancellationToken);

        return departments
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static DepartmentResponseDto MapToResponse(
        Department department)
    {
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            OfficeLocation = department.OfficeLocation,
            EstablishedDate = department.EstablishedDate,
            DepartmentHeadId = department.DepartmentHeadId,
            DepartmentHeadName =
                department.DepartmentHead?.FullName,
            IsActive = department.IsActive
        };
    }
}