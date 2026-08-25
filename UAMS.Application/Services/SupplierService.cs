using UAMS.Application.DTOs.Suppliers.Requests;
using UAMS.Application.DTOs.Suppliers.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Suppliers;

namespace UAMS.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public SupplierService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    private Guid GetCurrentUserId()
    {
        if (!_currentUserService.IsAuthenticated ||
            !_currentUserService.UserId.HasValue ||
            _currentUserService.UserId.Value == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Authenticated user is required.");
        }

        return _currentUserService.UserId.Value;
    }

    // ================================================================
    // Get Supplier By Id
    // ================================================================

    public async Task<SupplierResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(id, cancellationToken);

        if (supplier is null || supplier.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{id}' was not found.");
        }

        return MapToResponse(supplier);
    }


    // ================================================================
    // Get Supplier Details
    // ================================================================

    public async Task<SupplierDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var supplier = await _unitOfWork.Suppliers
            .GetByIdWithDetailsAsync(
                id,
                cancellationToken);

        if (supplier is null || supplier.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{id}' was not found.");
        }

        return new SupplierDetailResponseDto
        {
            Id = supplier.Id,
            Code = supplier.Code,
            Name = supplier.Name,
            ContactPerson = supplier.ContactPerson,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
            Address = supplier.Address,
            TaxIdentificationNumber =
                supplier.TaxIdentificationNumber,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt,
            PurchaseCount = supplier.Purchases.Count
        };
    }


    // ================================================================
    // Get Suppliers
    // ================================================================

    public async Task<SupplierListResponseDto> GetAllAsync(
        SupplierFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidatePagination(
            request.PageNumber,
            request.PageSize);

        var suppliers = await _unitOfWork.Suppliers
            .GetAllAsync(cancellationToken);

        IEnumerable<Supplier> query = suppliers
            .Where(supplier => !supplier.IsDeleted);


        // ============================================================
        // Search
        // ============================================================

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(supplier =>
                supplier.Name.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                supplier.Code.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                (
                    supplier.ContactPerson != null
                    &&
                    supplier.ContactPerson.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                )
                ||
                (
                    supplier.Email != null
                    &&
                    supplier.Email.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                )
                ||
                (
                    supplier.PhoneNumber != null
                    &&
                    supplier.PhoneNumber.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                ));
        }


        // ============================================================
        // Active Status
        // ============================================================

        if (request.IsActive.HasValue)
        {
            query = query.Where(
                supplier =>
                    supplier.IsActive == request.IsActive.Value);
        }


        // ============================================================
        // Ordering
        // ============================================================

        query = query
            .OrderBy(supplier => supplier.Name);


        // ============================================================
        // Pagination
        // ============================================================

        var totalCount = query.Count();

        var totalPages = CalculateTotalPages(
            totalCount,
            request.PageSize);

        var items = query
            .Skip(
                (request.PageNumber - 1)
                * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToResponse)
            .ToList();


        return new SupplierListResponseDto
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }


    // ================================================================
    // Create Supplier
    // ================================================================

    public async Task<SupplierResponseDto> CreateAsync(
        CreateSupplierRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // ============================================================
        // Validate duplicate name
        // ============================================================

        var existingByName = await _unitOfWork.Suppliers
            .GetByNameAsync(
                request.Name,
                cancellationToken);

        if (existingByName is not null
            && !existingByName.IsDeleted)
        {
            throw new InvalidOperationException(
                $"A supplier with the name '{request.Name}' already exists.");
        }


        // ============================================================
        // Validate duplicate code
        // ============================================================

        var existingByCode = await _unitOfWork.Suppliers
            .FindAsync(
                supplier =>
                    supplier.Code == request.Code,
                cancellationToken);

        if (existingByCode.Any(
                supplier => !supplier.IsDeleted))
        {
            throw new InvalidOperationException(
                $"A supplier with the code '{request.Code}' already exists.");
        }


        // ============================================================
        // Create Entity
        // ============================================================

        var supplier = Supplier.Create(
            request.Code,
            request.Name,
            request.ContactPerson,
            request.PhoneNumber,
            request.Email,
            request.Address,
            request.TaxIdentificationNumber);


        // ============================================================
        // Persist
        // ============================================================

        await _unitOfWork.Suppliers.AddAsync(
            supplier,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return MapToResponse(supplier);
    }


    // ================================================================
    // Update Supplier
    // ================================================================

    public async Task<SupplierResponseDto> UpdateAsync(
        Guid id,
        UpdateSupplierRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);


        // ============================================================
        // Get Existing Supplier
        // ============================================================

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(
                id,
                cancellationToken);

        if (supplier is null || supplier.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{id}' was not found.");
        }


        // ============================================================
        // Validate Duplicate Name
        // ============================================================

        var existingByName = await _unitOfWork.Suppliers
            .GetByNameAsync(
                request.Name,
                cancellationToken);

        if (existingByName is not null
            && existingByName.Id != id
            && !existingByName.IsDeleted)
        {
            throw new InvalidOperationException(
                $"A supplier with the name '{request.Name}' already exists.");
        }


        // ============================================================
        // Validate Duplicate Code
        // ============================================================

        var existingByCode = await _unitOfWork.Suppliers
            .FindAsync(
                existing =>
                    existing.Code == request.Code,
                cancellationToken);

        if (existingByCode.Any(
                existing =>
                    existing.Id != id
                    && !existing.IsDeleted))
        {
            throw new InvalidOperationException(
                $"A supplier with the code '{request.Code}' already exists.");
        }


        // ============================================================
        // Update Domain Entity
        // ============================================================

        supplier.Update(
            request.Code,
            request.Name,
            request.ContactPerson,
            request.PhoneNumber,
            request.Email,
            request.Address,
            request.TaxIdentificationNumber);


        // ============================================================
        // Update Status
        // ============================================================

        if (request.IsActive)
        {
            supplier.Activate();
        }
        else
        {
            supplier.Deactivate();
        }


        // ============================================================
        // Persist
        // ============================================================

        _unitOfWork.Suppliers.Update(supplier);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return MapToResponse(supplier);
    }


    // ================================================================
    // Activate Supplier
    // ================================================================

    public async Task ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(
                id,
                cancellationToken);

        if (supplier is null || supplier.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{id}' was not found.");
        }

        supplier.Activate();

        _unitOfWork.Suppliers.Update(supplier);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Deactivate Supplier
    // ================================================================

    public async Task DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var supplier = await _unitOfWork.Suppliers
            .GetByIdAsync(
                id,
                cancellationToken);

        if (supplier is null || supplier.IsDeleted)
        {
            throw new KeyNotFoundException(
                $"Supplier with ID '{id}' was not found.");
        }

        supplier.Deactivate();

        _unitOfWork.Suppliers.Update(supplier);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Delete Supplier
    // ================================================================

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Supplier ID is required.",
                nameof(id));
        }

        var supplier =
            await _unitOfWork.Suppliers.GetByIdAsync(
                id,
                cancellationToken);

        if (supplier is null)
        {
            throw new KeyNotFoundException(
                $"Supplier '{id}' was not found.");
        }

        var deletedBy = GetCurrentUserId();

        supplier.SoftDelete(deletedBy);

        _unitOfWork.Suppliers.Update(supplier);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static SupplierResponseDto MapToResponse(
        Supplier supplier)
    {
        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Code = supplier.Code,
            Name = supplier.Name,
            ContactPerson = supplier.ContactPerson,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
            Address = supplier.Address,
            TaxIdentificationNumber =
                supplier.TaxIdentificationNumber,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }


    // ================================================================
    // Validation Helpers
    // ================================================================

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Supplier ID is required.",
                nameof(id));
        }
    }


    private static void ValidatePagination(
        int pageNumber,
        int pageSize)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentException(
                "Page number must be greater than zero.",
                nameof(pageNumber));
        }

        if (pageSize <= 0 || pageSize > 100)
        {
            throw new ArgumentException(
                "Page size must be between 1 and 100.",
                nameof(pageSize));
        }
    }


    private static int CalculateTotalPages(
        int totalCount,
        int pageSize)
    {
        if (totalCount == 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(
            totalCount / (double)pageSize);
    }
}

