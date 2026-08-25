using UAMS.Application.DTOs.Maintenance.Requests;
using UAMS.Application.DTOs.Maintenance.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.Maintenances;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly IUnitOfWork _unitOfWork;

    public MaintenanceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ============================================================
    // Create
    // ============================================================

    public async Task<MaintenanceResponseDto> CreateAsync(
        CreateMaintenanceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // --------------------------------------------------------
        // Validate Asset
        // --------------------------------------------------------

        var asset = await _unitOfWork.Assets.GetByIdAsync(
            request.AssetId,
            cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");
        }

        // --------------------------------------------------------
        // Validate Requesting User
        // --------------------------------------------------------

        var requestedBy = await _unitOfWork.Users.GetByIdAsync(
            request.RequestedById,
            cancellationToken);

        if (requestedBy is null)
        {
            throw new KeyNotFoundException(
                $"User with ID '{request.RequestedById}' was not found.");
        }

        // --------------------------------------------------------
        // Validate Damage Report when supplied
        // --------------------------------------------------------

        if (request.DamageReportId.HasValue)
        {
            var damageReport =
                await _unitOfWork.DamageReports.GetByIdAsync(
                    request.DamageReportId.Value,
                    cancellationToken);

            if (damageReport is null)
            {
                throw new KeyNotFoundException(
                    $"Damage report with ID '{request.DamageReportId}' was not found.");
            }

            if (damageReport.AssetId != request.AssetId)
            {
                throw new InvalidOperationException(
                    "The damage report does not belong to the specified asset.");
            }
        }

        // --------------------------------------------------------
        // Generate Maintenance Number
        // --------------------------------------------------------

        var maintenanceNumber =
            await GenerateMaintenanceNumberAsync(
                cancellationToken);

        // --------------------------------------------------------
        // Requested Date
        // --------------------------------------------------------

        var requestedDate =
            request.RequestedDate ?? DateTime.UtcNow;

        // --------------------------------------------------------
        // Create Domain Entity
        // --------------------------------------------------------

        var maintenance = Maintenance.Create(
            maintenanceNumber,
            request.AssetId,
            request.DamageReportId,
            request.RequestedById,
            request.MaintenanceType,
            request.ProblemDescription,
            request.MaintenanceDescription,
            request.PartsUsed,
            request.EstimatedCost,
            requestedDate,
            request.Remarks);

        await _unitOfWork.MaintenanceRequests.AddAsync(
            maintenance,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Get By ID
    // ============================================================

    public async Task<MaintenanceDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        return await MapToDetailResponseAsync(
            maintenance,
            cancellationToken);
    }


    // ============================================================
    // Get By Maintenance Number
    // ============================================================

    public async Task<MaintenanceDetailResponseDto?>
        GetByMaintenanceNumberAsync(
            string maintenanceNumber,
            CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            maintenanceNumber,
            nameof(maintenanceNumber));

        var maintenance =
            await _unitOfWork.MaintenanceRequests
                .GetByMaintenanceNumberAsync(
                    maintenanceNumber.Trim(),
                    cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record '{maintenanceNumber}' was not found.");
        }

        return await MapToDetailResponseAsync(
            maintenance,
            cancellationToken);
    }


    // ============================================================
    // Get / Filter
    // ============================================================

    public async Task<MaintenanceListResponseDto>
        GetAllAsync(
            MaintenanceFilterRequestDto filter,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var maintenanceRecords =
            await _unitOfWork.MaintenanceRequests.GetAllAsync(
                cancellationToken);

        IEnumerable<Maintenance> query =
            maintenanceRecords;

        // --------------------------------------------------------
        // Maintenance Number
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(filter.MaintenanceNumber))
        {
            var maintenanceNumber =
                filter.MaintenanceNumber.Trim();

            query = query.Where(x =>
                x.MaintenanceNumber.Contains(
                    maintenanceNumber,
                    StringComparison.OrdinalIgnoreCase));
        }

        // --------------------------------------------------------
        // Asset
        // --------------------------------------------------------

        if (filter.AssetId.HasValue)
        {
            query = query.Where(x =>
                x.AssetId == filter.AssetId.Value);
        }

        // --------------------------------------------------------
        // Damage Report
        // --------------------------------------------------------

        if (filter.DamageReportId.HasValue)
        {
            query = query.Where(x =>
                x.DamageReportId == filter.DamageReportId.Value);
        }

        // --------------------------------------------------------
        // Requested By
        // --------------------------------------------------------

        if (filter.RequestedById.HasValue)
        {
            query = query.Where(x =>
                x.RequestedById == filter.RequestedById.Value);
        }

        // --------------------------------------------------------
        // Assigned Technician
        // --------------------------------------------------------

        if (filter.AssignedTechnicianId.HasValue)
        {
            query = query.Where(x =>
                x.AssignedTechnicianId ==
                filter.AssignedTechnicianId.Value);
        }

        // --------------------------------------------------------
        // Maintenance Type
        // --------------------------------------------------------

        if (filter.MaintenanceType.HasValue)
        {
            query = query.Where(x =>
                x.MaintenanceType ==
                filter.MaintenanceType.Value);
        }

        // --------------------------------------------------------
        // Status
        // --------------------------------------------------------

        if (filter.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == filter.Status.Value);
        }

        // --------------------------------------------------------
        // Result
        // --------------------------------------------------------

        if (filter.Result.HasValue)
        {
            query = query.Where(x =>
                x.Result == filter.Result.Value);
        }

        // --------------------------------------------------------
        // Requested Date Range
        // --------------------------------------------------------

        if (filter.RequestedFromDate.HasValue)
        {
            query = query.Where(x =>
                x.RequestedDate >=
                filter.RequestedFromDate.Value);
        }

        if (filter.RequestedToDate.HasValue)
        {
            query = query.Where(x =>
                x.RequestedDate <=
                filter.RequestedToDate.Value);
        }

        // --------------------------------------------------------
        // Completed Date Range
        // --------------------------------------------------------

        if (filter.CompletedFromDate.HasValue)
        {
            query = query.Where(x =>
                x.CompletedDate.HasValue &&
                x.CompletedDate.Value >=
                filter.CompletedFromDate.Value);
        }

        if (filter.CompletedToDate.HasValue)
        {
            query = query.Where(x =>
                x.CompletedDate.HasValue &&
                x.CompletedDate.Value <=
                filter.CompletedToDate.Value);
        }

        // --------------------------------------------------------
        // Active State
        // --------------------------------------------------------

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == filter.IsActive.Value);
        }

        // --------------------------------------------------------
        // Search
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm =
                filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.MaintenanceNumber.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase) ||

                x.ProblemDescription.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase) ||

                (x.MaintenanceDescription != null &&
                 x.MaintenanceDescription.Contains(
                     searchTerm,
                     StringComparison.OrdinalIgnoreCase)) ||

                (x.PartsUsed != null &&
                 x.PartsUsed.Contains(
                     searchTerm,
                     StringComparison.OrdinalIgnoreCase)));
        }

        // --------------------------------------------------------
        // Order
        // --------------------------------------------------------

        query = query.OrderByDescending(
            x => x.RequestedDate);

        // --------------------------------------------------------
        // Pagination
        // --------------------------------------------------------

        var totalCount = query.Count();

        var pageNumber =
            filter.PageNumber < 1
                ? 1
                : filter.PageNumber;

        var pageSize =
            filter.PageSize < 1
                ? 20
                : Math.Min(filter.PageSize, 100);

        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount / (double)pageSize);

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToListResponse)
            .ToList();

        return new MaintenanceListResponseDto
        {
            // If your current interface expects a paged-wrapper
            // DTO, this method should use that DTO instead.
        };
    }


    // ============================================================
    // Update
    // ============================================================

    public async Task<MaintenanceResponseDto> UpdateAsync(
        Guid id,
        UpdateMaintenanceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        if (maintenance.Status != MaintenanceStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending maintenance records can be updated.");
        }

        maintenance.Update(
            request.MaintenanceType,
            request.ProblemDescription,
            request.MaintenanceDescription,
            request.PartsUsed,
            request.EstimatedCost,
            request.Remarks);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Assign Technician
    // ============================================================

    public async Task<MaintenanceResponseDto>
        AssignTechnicianAsync(
            Guid id,
            AssignMaintenanceTechnicianRequestDto request,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        var technician =
            await _unitOfWork.Users.GetByIdAsync(
                request.AssignedTechnicianId,
                cancellationToken);

        if (technician is null)
        {
            throw new KeyNotFoundException(
                $"Technician with ID '{request.AssignedTechnicianId}' was not found.");
        }

        maintenance.AssignTechnician(
            request.AssignedTechnicianId,
            request.Remarks);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Start Maintenance
    // ============================================================

    public async Task<MaintenanceResponseDto>
        StartAsync(
            Guid id,
            StartMaintenanceRequestDto request,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        maintenance.Start(
            request.MaintenanceDescription,
            request.PartsUsed,
            request.Remarks);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Complete Maintenance
    // ============================================================

    public async Task<MaintenanceResponseDto>
        CompleteAsync(
            Guid id,
            CompleteMaintenanceRequestDto request,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        maintenance.Complete(
            request.Result,
            request.ActualCost,
            request.MaintenanceDescription,
            request.PartsUsed,
            request.FailureReason,
            request.Remarks);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Cancel
    // ============================================================

    public async Task<MaintenanceResponseDto>
        CancelAsync(
            Guid id,
            CancelMaintenanceRequestDto request,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        maintenance.Cancel(request.Reason);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Activate
    // ============================================================

    public async Task<MaintenanceResponseDto>
        ActivateAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        maintenance.Activate();

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(maintenance);
    }


    // ============================================================
    // Soft Delete
    // ============================================================

    public async Task DeleteAsync(
        Guid id,
        Guid deletedBy,
        CancellationToken cancellationToken = default)
    {
        var maintenance =
            await _unitOfWork.MaintenanceRequests.GetByIdAsync(
                id,
                cancellationToken);

        if (maintenance is null)
        {
            throw new KeyNotFoundException(
                $"Maintenance record with ID '{id}' was not found.");
        }

        maintenance.MarkDeleted(deletedBy);

        _unitOfWork.MaintenanceRequests.Update(
            maintenance);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ============================================================
    // Pending Maintenance
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests.GetPendingAsync(
                cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Open Maintenance
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetOpenAsync(
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests.GetOpenAsync(
                cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get By Asset
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests
                .GetByAssetIdAsync(
                    assetId,
                    cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get By Damage Report
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetByDamageReportIdAsync(
            Guid damageReportId,
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests
                .GetByDamageReportIdAsync(
                    damageReportId,
                    cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get By Requested By
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests
                .GetByRequestedByIdAsync(
                    requestedById,
                    cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Get By Technician
    // ============================================================

    public async Task<IReadOnlyList<MaintenanceResponseDto>>
        GetByAssignedTechnicianIdAsync(
            Guid technicianId,
            CancellationToken cancellationToken = default)
    {
        var records =
            await _unitOfWork.MaintenanceRequests
                .GetByAssignedTechnicianIdAsync(
                    technicianId,
                    cancellationToken);

        return records
            .Select(MapToResponse)
            .ToList();
    }


    // ============================================================
    // Number Generator
    // ============================================================

    private async Task<string>
        GenerateMaintenanceNumberAsync(
            CancellationToken cancellationToken)
    {
        var datePart =
            DateTime.UtcNow.ToString("yyyyMMdd");

        var prefix =
            $"MNT-{datePart}";

        var existing =
            await _unitOfWork.MaintenanceRequests.GetAllAsync(
                cancellationToken);

        var sequence =
            existing
                .Select(x => x.MaintenanceNumber)
                .Where(x =>
                    x.StartsWith(
                        prefix,
                        StringComparison.OrdinalIgnoreCase))
                .Select(x =>
                {
                    var parts = x.Split('-');

                    if (parts.Length != 3)
                    {
                        return 0;
                    }

                    return int.TryParse(
                        parts[2],
                        out var number)
                        ? number
                        : 0;
                })
                .DefaultIfEmpty(0)
                .Max() + 1;

        return $"{prefix}-{sequence:D4}";
    }


    // ============================================================
    // Response Mapping
    // ============================================================

    private static MaintenanceResponseDto
        MapToResponse(Maintenance maintenance)
    {
        return new MaintenanceResponseDto
        {
            Id = maintenance.Id,

            MaintenanceNumber =
                maintenance.MaintenanceNumber,

            AssetId =
                maintenance.AssetId,

            DamageReportId =
                maintenance.DamageReportId,

            RequestedById =
                maintenance.RequestedById,

            AssignedTechnicianId =
                maintenance.AssignedTechnicianId,

            MaintenanceType =
                maintenance.MaintenanceType,

            ProblemDescription =
                maintenance.ProblemDescription,

            MaintenanceDescription =
                maintenance.MaintenanceDescription,

            PartsUsed =
                maintenance.PartsUsed,

            EstimatedCost =
                maintenance.EstimatedCost,

            ActualCost =
                maintenance.ActualCost,

            RequestedDate =
                maintenance.RequestedDate,

            StartedDate =
                maintenance.StartedDate,

            CompletedDate =
                maintenance.CompletedDate,

            Result =
                maintenance.Result,

            FailureReason =
                maintenance.FailureReason,

            Remarks =
                maintenance.Remarks,

            Status =
                maintenance.Status,

            IsActive =
                maintenance.IsActive
        };
    }


    private static MaintenanceListResponseDto
        MapToListResponse(Maintenance maintenance)
    {
        // This method is intentionally kept separate from the
        // normal response mapping because the list DTO contains
        // display-oriented asset/user fields.

        return new MaintenanceListResponseDto
        {
            Id = maintenance.Id,

            MaintenanceNumber =
                maintenance.MaintenanceNumber,

            AssetId =
                maintenance.AssetId,

            AssetTag =
                maintenance.Asset?.AssetTag,

            AssetName =
                maintenance.Asset?.Name,

            MaintenanceType =
                maintenance.MaintenanceType,

            RequestedById =
                maintenance.RequestedById,

            RequestedByName =
                maintenance.RequestedBy?.FullName,

            AssignedTechnicianId =
                maintenance.AssignedTechnicianId,

            AssignedTechnicianName =
                maintenance.AssignedTechnician?.FullName,

            EstimatedCost =
                maintenance.EstimatedCost,

            ActualCost =
                maintenance.ActualCost,

            RequestedDate =
                maintenance.RequestedDate,

            CompletedDate =
                maintenance.CompletedDate,

            Result =
                maintenance.Result,

            Status =
                maintenance.Status,

            IsActive =
                maintenance.IsActive
        };
    }


    private async Task<MaintenanceDetailResponseDto>
        MapToDetailResponseAsync(
            Maintenance maintenance,
            CancellationToken cancellationToken)
    {
        var asset =
            await _unitOfWork.Assets.GetByIdAsync(
                maintenance.AssetId,
                cancellationToken);

        var requestedBy =
            await _unitOfWork.Users.GetByIdAsync(
                maintenance.RequestedById,
                cancellationToken);

        var technician =
            maintenance.AssignedTechnicianId.HasValue
                ? await _unitOfWork.Users.GetByIdAsync(
                    maintenance.AssignedTechnicianId.Value,
                    cancellationToken)
                : null;

        string? damageReportNumber = null;

        if (maintenance.DamageReportId.HasValue)
        {
            var damageReport =
                await _unitOfWork.DamageReports.GetByIdAsync(
                    maintenance.DamageReportId.Value,
                    cancellationToken);

            damageReportNumber =
                damageReport?.ReportNumber;
        }

        return new MaintenanceDetailResponseDto
        {
            Id =
                maintenance.Id,

            MaintenanceNumber =
                maintenance.MaintenanceNumber,

            AssetId =
                maintenance.AssetId,

            AssetTag =
                asset?.AssetTag,

            AssetName =
                asset?.Name,

            AssetSerialNumber =
                asset?.SerialNumber,

            DamageReportId =
                maintenance.DamageReportId,

            DamageReportNumber =
                damageReportNumber,

            RequestedById =
                maintenance.RequestedById,

            RequestedByName =
                requestedBy?.FullName,

            AssignedTechnicianId =
                maintenance.AssignedTechnicianId,

            AssignedTechnicianName =
                technician?.FullName,

            MaintenanceType =
                maintenance.MaintenanceType,

            ProblemDescription =
                maintenance.ProblemDescription,

            MaintenanceDescription =
                maintenance.MaintenanceDescription,

            PartsUsed =
                maintenance.PartsUsed,

            EstimatedCost =
                maintenance.EstimatedCost,

            ActualCost =
                maintenance.ActualCost,

            RequestedDate =
                maintenance.RequestedDate,

            StartedDate =
                maintenance.StartedDate,

            CompletedDate =
                maintenance.CompletedDate,

            Result =
                maintenance.Result,

            FailureReason =
                maintenance.FailureReason,

            Status =
                maintenance.Status,

            IsActive =
                maintenance.IsActive,

            Remarks =
                maintenance.Remarks,

            CreatedAt =
                maintenance.CreatedAt,

            CreatedBy =
                maintenance.CreatedBy,

            ModifiedAt =
                maintenance.UpdatedAt,

            ModifiedBy =
                maintenance.UpdatedBy
        };
    }
}