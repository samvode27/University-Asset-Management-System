using UAMS.Application.DTOs.AssetRequests.Requests;
using UAMS.Application.DTOs.AssetRequests.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class AssetRequestService : IAssetRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }




    // ================================================================
    // Get Asset Request By ID
    // ================================================================

    public async Task<AssetRequestResponseDto> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var request = await _unitOfWork.AssetRequests
            .GetByIdAsync(
                id,
                cancellationToken);

        if (request is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        return MapToResponse(request);
    }


    // ================================================================
    // Get Asset Request Details
    // ================================================================

    public async Task<AssetRequestDetailResponseDto> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var request = await _unitOfWork.AssetRequests
            .GetByIdAsync(
                id,
                cancellationToken);

        if (request is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        return MapToDetailResponse(request);
    }


    // ================================================================
    // Get By Request Number
    // ================================================================

    public async Task<AssetRequestResponseDto> GetByRequestNumberAsync(
        string requestNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestNumber);

        var normalizedRequestNumber =
            requestNumber.Trim();

        var request = await _unitOfWork.AssetRequests
            .GetByRequestNumberAsync(
                normalizedRequestNumber,
                cancellationToken);

        if (request is null)
        {
            throw new KeyNotFoundException(
                $"Asset request '{normalizedRequestNumber}' was not found.");
        }

        return MapToResponse(request);
    }


    // ================================================================
    // Get By Requester
    // ================================================================

    public async Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByRequesterIdAsync(
            Guid requesterId,
            CancellationToken cancellationToken = default)
    {
        ValidateRequesterId(requesterId);

        var requests = await _unitOfWork.AssetRequests
            .GetByRequesterIdAsync(
                requesterId,
                cancellationToken);

        return requests
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    public async Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        ValidateAssetId(assetId);

        var requests = await _unitOfWork.AssetRequests
            .GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return requests
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Department
    // ================================================================

    public async Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByDepartmentIdAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default)
    {
        ValidateDepartmentId(departmentId);

        var requests = await _unitOfWork.AssetRequests
            .GetByDepartmentIdAsync(
                departmentId,
                cancellationToken);

        return requests
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Status
    // ================================================================

    public async Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByStatusAsync(
            AssetRequestStatus status,
            CancellationToken cancellationToken = default)
    {
        ValidateStatus(status);

        var requests = await _unitOfWork.AssetRequests
            .GetByStatusAsync(
                status,
                cancellationToken);

        return requests
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Requester And Status
    // ================================================================

    public async Task<IReadOnlyList<AssetRequestResponseDto>>
        GetByRequesterAndStatusAsync(
            Guid requesterId,
            AssetRequestStatus status,
            CancellationToken cancellationToken = default)
    {
        ValidateRequesterId(requesterId);

        ValidateStatus(status);

        var requests = await _unitOfWork.AssetRequests
            .GetByRequesterAndStatusAsync(
                requesterId,
                status,
                cancellationToken);

        return requests
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get All Asset Requests
    // ================================================================

    public async Task<AssetRequestListResponseDto> GetAllAsync(
        AssetRequestFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requests = await _unitOfWork.AssetRequests
            .GetAllAsync(cancellationToken);

        IEnumerable<AssetRequest> query = requests;

        query =
            ApplyFilters(
                query,
                request);

        query =
            ApplyOrdering(
                query,
                request);

        var totalCount =
            query.Count();

        var totalPages =
            CalculateTotalPages(
                totalCount,
                request.PageSize);

        var items =
            query
                .Skip(
                    (request.PageNumber - 1) *
                    request.PageSize)
                .Take(request.PageSize)
                .Select(MapToResponse)
                .ToList();

        return new AssetRequestListResponseDto
        {
            Items = items,

            PageNumber =
                request.PageNumber,

            PageSize =
                request.PageSize,

            TotalCount =
                totalCount,

            TotalPages =
                totalPages,

            HasPreviousPage =
                request.PageNumber > 1,

            HasNextPage =
                request.PageNumber < totalPages
        };
    }


    // ================================================================
    // Create Asset Request
    // ================================================================

    public async Task<AssetRequestResponseDto> CreateAsync(
        CreateAssetRequestDto request,
        Guid requesterId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateRequesterId(requesterId);

        ValidateAssetId(request.AssetId);

        ValidateDepartmentId(request.DepartmentId);

        // ------------------------------------------------------------
        // Verify requester
        // ------------------------------------------------------------

        var requester =
            await _unitOfWork.Users
                .GetByIdAsync(
                    requesterId,
                    cancellationToken);

        if (requester is null)
        {
            throw new KeyNotFoundException(
                $"Requester with ID '{requesterId}' was not found.");
        }

        if (!requester.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive user cannot create an asset request.");
        }

        // ------------------------------------------------------------
        // Verify asset
        // ------------------------------------------------------------

        var asset =
            await _unitOfWork.Assets
                .GetByIdAsync(
                    request.AssetId,
                    cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");
        }

        if (!asset.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive asset cannot be requested.");
        }

        // ------------------------------------------------------------
        // Verify department
        // ------------------------------------------------------------

        var department =
            await _unitOfWork.Departments
                .GetByIdAsync(
                    request.DepartmentId,
                    cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                $"Department with ID '{request.DepartmentId}' was not found.");
        }

        // ------------------------------------------------------------
        // Prevent duplicate pending request
        // ------------------------------------------------------------

        var existingRequests =
            await _unitOfWork.AssetRequests
                .GetByRequesterAndStatusAsync(
                    requesterId,
                    AssetRequestStatus.PendingDepartmentHeadApproval,
                    cancellationToken);

        var duplicatePendingRequest =
            existingRequests.Any(existing =>
                existing.AssetId ==
                request.AssetId);

        if (duplicatePendingRequest)
        {
            throw new InvalidOperationException(
                "The requester already has a pending request for this asset.");
        }

        // ------------------------------------------------------------
        // Generate request number
        // ------------------------------------------------------------

        var requestNumber =
            await GenerateRequestNumberAsync(
                cancellationToken);

        var requestedDate =
            DateTime.UtcNow;

        // ------------------------------------------------------------
        // Create entity
        // ------------------------------------------------------------

        var assetRequest =
            AssetRequest.Create(
                requestNumber,
                request.AssetId,
                requesterId,
                request.DepartmentId,
                request.Purpose,
                requestedDate,
                request.RequiredFromDate,
                request.RequiredToDate);

        await _unitOfWork.AssetRequests
            .AddAsync(
                assetRequest,
                cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assetRequest);
    }


    // ================================================================
    // Update Asset Request
    // ================================================================

    public async Task<AssetRequestResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);

        ValidateAssetId(request.AssetId);

        ValidateDepartmentId(request.DepartmentId);

        var assetRequest =
            await _unitOfWork.AssetRequests
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assetRequest is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        // ------------------------------------------------------------
        // Verify asset
        // ------------------------------------------------------------

        var asset =
            await _unitOfWork.Assets
                .GetByIdAsync(
                    request.AssetId,
                    cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset with ID '{request.AssetId}' was not found.");
        }

        if (!asset.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive asset cannot be requested.");
        }

        // ------------------------------------------------------------
        // Verify department
        // ------------------------------------------------------------

        var department =
            await _unitOfWork.Departments
                .GetByIdAsync(
                    request.DepartmentId,
                    cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                $"Department with ID '{request.DepartmentId}' was not found.");
        }

        // ------------------------------------------------------------
        // Entity validates workflow state
        // ------------------------------------------------------------

        assetRequest.Update(
            request.AssetId,
            request.DepartmentId,
            request.Purpose,
            request.RequiredFromDate,
            request.RequiredToDate);

        _unitOfWork.AssetRequests
            .Update(assetRequest);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assetRequest);
    }


    // ================================================================
    // Department Head Review
    // ================================================================

    public async Task<AssetRequestApprovalResponseDto>
        ReviewByDepartmentHeadAsync(
            Guid id,
            DepartmentHeadReviewRequestDto request,
            Guid departmentHeadId,
            CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ValidateUserId(
            departmentHeadId,
            "Department Head ID");

        ArgumentNullException.ThrowIfNull(request);

        // ------------------------------------------------------------
        // Get request
        // ------------------------------------------------------------

        var assetRequest =
            await _unitOfWork.AssetRequests
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assetRequest is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        // ------------------------------------------------------------
        // Verify Department Head
        // ------------------------------------------------------------

        var departmentHead =
            await _unitOfWork.Users
                .GetByIdAsync(
                    departmentHeadId,
                    cancellationToken);

        if (departmentHead is null)
        {
            throw new KeyNotFoundException(
                $"Department Head with ID '{departmentHeadId}' was not found.");
        }

        if (!departmentHead.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive user cannot review asset requests.");
        }

        // ------------------------------------------------------------
        // Verify Department Head belongs to request department
        // ------------------------------------------------------------

        await ValidateDepartmentHeadAccessAsync(
            assetRequest.DepartmentId,
            departmentHeadId,
            cancellationToken);

        // ------------------------------------------------------------
        // Domain state transition
        // ------------------------------------------------------------

        var actionDate =
            DateTime.UtcNow;

        assetRequest.ReviewByDepartmentHead(
            departmentHeadId,
            request.Approved,
            actionDate,
            request.Remarks);

        _unitOfWork.AssetRequests
            .Update(assetRequest);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new AssetRequestApprovalResponseDto
        {
            Id =
                assetRequest.Id,

            RequestNumber =
                assetRequest.RequestNumber,

            Status =
                assetRequest.Status,

            Approved =
                request.Approved,

            ActionedById =
                departmentHeadId,

            ActionedByName =
                GetUserDisplayName(departmentHead),

            ActionDate =
                actionDate,

            Remarks =
                request.Remarks,

            RejectionReason =
                request.Approved
                    ? null
                    : request.Remarks,

            RequiresNextApproval =
                assetRequest.Status ==
                AssetRequestStatus.PendingAssetManagerApproval,

            ReadyForAssignment =
                assetRequest.IsReadyForAssignment()
        };
    }


    // ================================================================
    // Asset Manager Review
    // ================================================================

    public async Task<AssetRequestApprovalResponseDto>
        ReviewByAssetManagerAsync(
            Guid id,
            AssetManagerReviewRequestDto request,
            Guid assetManagerId,
            CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ValidateUserId(
            assetManagerId,
            "Asset Manager ID");

        ArgumentNullException.ThrowIfNull(request);

        // ------------------------------------------------------------
        // Get request
        // ------------------------------------------------------------

        var assetRequest =
            await _unitOfWork.AssetRequests
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assetRequest is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        // ------------------------------------------------------------
        // Verify Asset Manager
        // ------------------------------------------------------------

        var assetManager =
            await _unitOfWork.Users
                .GetByIdAsync(
                    assetManagerId,
                    cancellationToken);

        if (assetManager is null)
        {
            throw new KeyNotFoundException(
                $"Asset Manager with ID '{assetManagerId}' was not found.");
        }

        if (!assetManager.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive user cannot review asset requests.");
        }

        // ------------------------------------------------------------
        // Domain state transition
        // ------------------------------------------------------------

        var actionDate =
            DateTime.UtcNow;

        assetRequest.ReviewByAssetManager(
            assetManagerId,
            request.Approved,
            actionDate,
            request.Remarks);

        _unitOfWork.AssetRequests
            .Update(assetRequest);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new AssetRequestApprovalResponseDto
        {
            Id =
                assetRequest.Id,

            RequestNumber =
                assetRequest.RequestNumber,

            Status =
                assetRequest.Status,

            Approved =
                request.Approved,

            ActionedById =
                assetManagerId,

            ActionedByName =
                GetUserDisplayName(assetManager),

            ActionDate =
                actionDate,

            Remarks =
                request.Remarks,

            RejectionReason =
                request.Approved
                    ? null
                    : request.Remarks,

            RequiresNextApproval =
                false,

            ReadyForAssignment =
                assetRequest.IsReadyForAssignment()
        };
    }


    // ================================================================
    // Cancel Asset Request
    // ================================================================

    public async Task CancelAsync(
        Guid id,
        CancelAssetRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        ArgumentNullException.ThrowIfNull(request);

        var assetRequest =
            await _unitOfWork.AssetRequests
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assetRequest is null)
        {
            throw new KeyNotFoundException(
                $"Asset request with ID '{id}' was not found.");
        }

        assetRequest.Cancel(
            request.Reason);

        _unitOfWork.AssetRequests
            .Update(assetRequest);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ================================================================
    // Filtering
    // ================================================================

    private static IEnumerable<AssetRequest> ApplyFilters(
        IEnumerable<AssetRequest> query,
        AssetRequestFilterRequestDto request)
    {
        if (!string.IsNullOrWhiteSpace(request.RequestNumber))
        {
            var requestNumber =
                request.RequestNumber.Trim();

            query = query.Where(assetRequest =>
                assetRequest.RequestNumber.Contains(
                    requestNumber,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.AssetId.HasValue)
        {
            query = query.Where(assetRequest =>
                assetRequest.AssetId ==
                request.AssetId.Value);
        }

        if (request.RequesterId.HasValue)
        {
            query = query.Where(assetRequest =>
                assetRequest.RequesterId ==
                request.RequesterId.Value);
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(assetRequest =>
                assetRequest.DepartmentId ==
                request.DepartmentId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(assetRequest =>
                assetRequest.Status ==
                request.Status.Value);
        }

        if (request.RequestedFrom.HasValue)
        {
            query = query.Where(assetRequest =>
                assetRequest.RequestedDate >=
                request.RequestedFrom.Value);
        }

        if (request.RequestedTo.HasValue)
        {
            var endDateExclusive =
                request.RequestedTo.Value.Date.AddDays(1);

            query = query.Where(assetRequest =>
                assetRequest.RequestedDate <
                endDateExclusive);
        }

        if (request.RequiresDepartmentHeadAction.HasValue)
        {
            query =
                request.RequiresDepartmentHeadAction.Value
                    ? query.Where(assetRequest =>
                        assetRequest.Status ==
                        AssetRequestStatus.PendingDepartmentHeadApproval)
                    : query.Where(assetRequest =>
                        assetRequest.Status !=
                        AssetRequestStatus.PendingDepartmentHeadApproval);
        }

        if (request.RequiresAssetManagerAction.HasValue)
        {
            query =
                request.RequiresAssetManagerAction.Value
                    ? query.Where(assetRequest =>
                        assetRequest.Status ==
                        AssetRequestStatus.PendingAssetManagerApproval)
                    : query.Where(assetRequest =>
                        assetRequest.Status !=
                        AssetRequestStatus.PendingAssetManagerApproval);
        }

        return query;
    }


    // ================================================================
    // Ordering
    // ================================================================

    private static IEnumerable<AssetRequest> ApplyOrdering(
        IEnumerable<AssetRequest> query,
        AssetRequestFilterRequestDto request)
    {
        return request.SortBy?
            .Trim()
            .ToLowerInvariant() switch
        {
            "requestnumber" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        assetRequest =>
                            assetRequest.RequestNumber)
                    : query.OrderBy(
                        assetRequest =>
                            assetRequest.RequestNumber),

            "requesteddate" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        assetRequest =>
                            assetRequest.RequestedDate)
                    : query.OrderBy(
                        assetRequest =>
                            assetRequest.RequestedDate),

            "status" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        assetRequest =>
                            assetRequest.Status)
                    : query.OrderBy(
                        assetRequest =>
                            assetRequest.Status),

            "requiredfromdate" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        assetRequest =>
                            assetRequest.RequiredFromDate)
                    : query.OrderBy(
                        assetRequest =>
                            assetRequest.RequiredFromDate),

            "requiredtodate" =>
                request.SortDescending
                    ? query.OrderByDescending(
                        assetRequest =>
                            assetRequest.RequiredToDate)
                    : query.OrderBy(
                        assetRequest =>
                            assetRequest.RequiredToDate),

            _ =>
                query.OrderByDescending(
                    assetRequest =>
                        assetRequest.RequestedDate)
        };
    }


    // ================================================================
    // Request Number
    // ================================================================

    private async Task<string> GenerateRequestNumberAsync(
        CancellationToken cancellationToken)
    {
        var datePart =
            DateTime.UtcNow.ToString("yyyyMMdd");

        var prefix =
            $"AR-{datePart}-";

        var existingRequests =
            await _unitOfWork.AssetRequests
                .GetAllAsync(cancellationToken);

        var sequence =
            existingRequests
                .Select(request =>
                    request.RequestNumber)
                .Where(requestNumber =>
                    requestNumber.StartsWith(
                        prefix,
                        StringComparison.OrdinalIgnoreCase))
                .Select(requestNumber =>
                {
                    var sequencePart =
                        requestNumber[
                            prefix.Length..];

                    return int.TryParse(
                        sequencePart,
                        out var number)
                            ? number
                            : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

        return
            $"{prefix}{sequence + 1:D5}";
    }


    // ================================================================
    // Department Head Validation
    // ================================================================

    private async Task ValidateDepartmentHeadAccessAsync(
        Guid departmentId,
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var department =
            await _unitOfWork.Departments
                .GetByIdAsync(
                    departmentId,
                    cancellationToken);

        if (department is null)
        {
            throw new KeyNotFoundException(
                $"Department with ID '{departmentId}' was not found.");
        }

        // The exact Department Head relationship depends on
        // the current Department entity design.
        //
        // Do not infer or modify that relationship here until
        // the Department entity exposes the corresponding property.
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static AssetRequestResponseDto MapToResponse(
        AssetRequest request)
    {
        return new AssetRequestResponseDto
        {
            Id =
                request.Id,

            RequestNumber =
                request.RequestNumber,

            AssetId =
                request.AssetId,

            RequesterId =
                request.RequesterId,

            DepartmentId =
                request.DepartmentId,

            Purpose =
                request.Purpose,

            RequestedDate =
                request.RequestedDate,

            RequiredFromDate =
                request.RequiredFromDate,

            RequiredToDate =
                request.RequiredToDate,

            Status =
                request.Status,

            IsActive =
                request.IsActive,

            CreatedAt =
                request.CreatedAt,

            UpdatedAt =
                request.UpdatedAt
        };
    }


    private static AssetRequestDetailResponseDto
        MapToDetailResponse(
            AssetRequest request)
    {
        return new AssetRequestDetailResponseDto
        {
            Id =
                request.Id,

            RequestNumber =
                request.RequestNumber,

            Purpose =
                request.Purpose,

            RequestedDate =
                request.RequestedDate,

            RequiredFromDate =
                request.RequiredFromDate,

            RequiredToDate =
                request.RequiredToDate,

            Status =
                request.Status,

            IsActive =
                request.IsActive,

            AssetId =
                request.AssetId,

            AssetTag =
                request.Asset?.AssetTag,

            AssetName =
                request.Asset?.Name,

            SerialNumber =
                request.Asset?.SerialNumber,

            AssetStatus =
                request.Asset?.Status.ToString(),

            RequesterId =
                request.RequesterId,

            RequesterName =
                GetUserDisplayName(request.Requester),

            RequesterEmail =
                request.Requester?.Email,

            DepartmentId =
                request.DepartmentId,

            DepartmentName =
                request.Department?.Name,

            DepartmentHeadId =
                request.DepartmentHeadId,

            DepartmentHeadName =
                GetUserDisplayName(request.DepartmentHead),

            DepartmentHeadActionDate =
                request.DepartmentHeadActionDate,

            DepartmentHeadRemarks =
                request.DepartmentHeadRemarks,

            AssetManagerId =
                request.AssetManagerId,

            AssetManagerName =
                GetUserDisplayName(request.AssetManager),

            AssetManagerActionDate =
                request.AssetManagerActionDate,

            AssetManagerRemarks =
                request.AssetManagerRemarks,

            RejectionReason =
                request.RejectionReason,

            CreatedAt =
                request.CreatedAt,

            UpdatedAt =
                request.UpdatedAt
        };
    }


    // ================================================================
    // User Display Name
    // ================================================================

    private static string? GetUserDisplayName(
        UAMS.Domain.Entities.Users.User? user)
    {
        if (user is null)
        {
            return null;
        }

        return user.FullName;
    }


    // ================================================================
    // Pagination
    // ================================================================

    private static int CalculateTotalPages(
        int totalCount,
        int pageSize)
    {
        if (totalCount == 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(
            totalCount /
            (double)pageSize);
    }


    // ================================================================
    // Validation
    // ================================================================

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset request ID is required.",
                nameof(id));
        }
    }


    private static void ValidateRequesterId(
        Guid requesterId)
    {
        ValidateUserId(
            requesterId,
            "Requester ID");
    }


    private static void ValidateUserId(
        Guid userId,
        string parameterName)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                $"{parameterName} is required.",
                parameterName);
        }
    }


    private static void ValidateAssetId(
        Guid assetId)
    {
        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }
    }


    private static void ValidateDepartmentId(
        Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Department ID is required.",
                nameof(departmentId));
        }
    }


    private static void ValidateStatus(
        AssetRequestStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentException(
                "Invalid asset request status.",
                nameof(status));
        }
    }
}