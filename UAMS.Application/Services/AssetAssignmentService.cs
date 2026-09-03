using UAMS.Application.DTOs.AssetAssignments.Requests;
using UAMS.Application.DTOs.AssetAssignments.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetAssignments;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class AssetAssignmentService
    : IAssetAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetAssignmentService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork =
            unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }



    // ================================================================
    // Create
    // ================================================================

    public async Task<AssetAssignmentResponseDto> CreateAsync(
        CreateAssetAssignmentRequestDto request,
        Guid assignedById,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (assignedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned by user ID is required.",
                nameof(assignedById));
        }

        // ------------------------------------------------------------
        // Validate Asset
        // ------------------------------------------------------------

        var asset =
            await _unitOfWork.Assets.GetByIdAsync(
                request.AssetId,
                cancellationToken);

        if (asset is null)
        {
            throw new KeyNotFoundException(
                $"Asset '{request.AssetId}' was not found.");
        }

        // ------------------------------------------------------------
        // Validate Asset Request
        // ------------------------------------------------------------

        var assetRequest =
            await _unitOfWork.AssetRequests.GetByIdAsync(
                request.AssetRequestId,
                cancellationToken);

        if (assetRequest is null)
        {
            throw new KeyNotFoundException(
                $"Asset request '{request.AssetRequestId}' was not found.");
        }

        // ------------------------------------------------------------
        // Validate Employee
        // ------------------------------------------------------------

        var employee =
            await _unitOfWork.Users.GetByIdAsync(
                request.EmployeeId,
                cancellationToken);

        if (employee is null)
        {
            throw new KeyNotFoundException(
                $"Employee '{request.EmployeeId}' was not found.");
        }

        // ------------------------------------------------------------
        // Validate Assigned By
        // ------------------------------------------------------------

        var assignedBy =
            await _unitOfWork.Users.GetByIdAsync(
                assignedById,
                cancellationToken);

        if (assignedBy is null)
        {
            throw new KeyNotFoundException(
                $"Assigning user '{assignedById}' was not found.");
        }

        // ------------------------------------------------------------
        // Check Existing Assignment For Request
        // ------------------------------------------------------------

        var existingAssignment =
            await _unitOfWork.AssetAssignments
                .GetByAssetRequestIdAsync(
                    request.AssetRequestId,
                    cancellationToken);

        if (existingAssignment is not null)
        {
            throw new InvalidOperationException(
                "An asset assignment already exists for this asset request.");
        }

        // ------------------------------------------------------------
        // Check Active Assignment For Asset
        // ------------------------------------------------------------

        var activeAssignment =
            await _unitOfWork.AssetAssignments
                .GetActiveByAssetIdAsync(
                    request.AssetId,
                    cancellationToken);

        if (activeAssignment is not null)
        {
            throw new InvalidOperationException(
                "The asset already has an active assignment.");
        }

        // ------------------------------------------------------------
        // Validate Asset Request Status
        // ------------------------------------------------------------

        if (assetRequest.Status !=
            AssetRequestStatus.AssetManagerApproved)
        {
            throw new InvalidOperationException(
                "Only asset requests approved by the Asset Manager can be assigned.");
        }

        // ------------------------------------------------------------
        // Validate Asset Status
        // ------------------------------------------------------------

        if (asset.Status != AssetStatus.Available)
        {
            throw new InvalidOperationException(
                "Only available assets can be assigned.");
        }

        // ------------------------------------------------------------
        // Generate Assignment Number
        // ------------------------------------------------------------

        var assignmentNumber =
            await GenerateAssignmentNumberAsync(
                cancellationToken);

        // ------------------------------------------------------------
        // Normalize Expected Return Date
        // ------------------------------------------------------------

        DateTime? expectedReturnDate =
            request.ExpectedReturnDate.HasValue
                ? DateTime.SpecifyKind(
                    request.ExpectedReturnDate.Value,
                    DateTimeKind.Utc)
                : null;

        // ------------------------------------------------------------
        // Create Entity
        // ------------------------------------------------------------

        var assignment =
            AssetAssignment.Create(
                assignmentNumber,
                request.AssetId,
                request.AssetRequestId,
                request.EmployeeId,
                assignedById,
                DateTime.UtcNow,
                expectedReturnDate,
                request.AssignmentLocation,
                request.ConditionAtAssignment,
                request.Remarks);

        // ------------------------------------------------------------
        // Update Asset Lifecycle
        // ------------------------------------------------------------

        asset.Assign();

        // ------------------------------------------------------------
        // Persist
        // ------------------------------------------------------------

        await _unitOfWork.AssetAssignments.AddAsync(
            assignment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assignment);
    }


    // ================================================================
    // Get By Id
    // ================================================================

    public async Task<AssetAssignmentDetailResponseDto?>
        GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Assignment ID is required.",
                nameof(id));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assignment is null)
        {
            return null;
        }

        return await MapToDetailResponseAsync(
            assignment,
            cancellationToken);
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    public async Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        var assignments =
            await _unitOfWork.AssetAssignments
                .GetByAssetIdAsync(
                    assetId,
                    cancellationToken);

        return assignments
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Employee
    // ================================================================

    public async Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException(
                "Employee ID is required.",
                nameof(employeeId));
        }

        var assignments =
            await _unitOfWork.AssetAssignments
                .GetByEmployeeIdAsync(
                    employeeId,
                    cancellationToken);

        return assignments
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Asset Request
    // ================================================================

    public async Task<AssetAssignmentResponseDto?>
        GetByAssetRequestIdAsync(
            Guid assetRequestId,
            CancellationToken cancellationToken = default)
    {
        if (assetRequestId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset request ID is required.",
                nameof(assetRequestId));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetByAssetRequestIdAsync(
                    assetRequestId,
                    cancellationToken);

        return assignment is null
            ? null
            : MapToResponse(assignment);
    }


    // ================================================================
    // Get Active By Asset
    // ================================================================

    public async Task<AssetAssignmentResponseDto?>
        GetActiveByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        if (assetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Asset ID is required.",
                nameof(assetId));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetActiveByAssetIdAsync(
                    assetId,
                    cancellationToken);

        return assignment is null
            ? null
            : MapToResponse(assignment);
    }


    // ================================================================
    // Get Active By Employee
    // ================================================================

    public async Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetActiveByEmployeeIdAsync(
            Guid employeeId,
            CancellationToken cancellationToken = default)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException(
                "Employee ID is required.",
                nameof(employeeId));
        }

        var assignments =
            await _unitOfWork.AssetAssignments
                .GetActiveByEmployeeIdAsync(
                    employeeId,
                    cancellationToken);

        return assignments
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Status
    // ================================================================

    public async Task<IReadOnlyList<AssetAssignmentResponseDto>>
        GetByStatusAsync(
            AssetAssignmentStatus status,
            CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentException(
                "Invalid asset assignment status.",
                nameof(status));
        }

        var assignments =
            await _unitOfWork.AssetAssignments
                .FindAsync(
                    assignment =>
                        assignment.Status == status,
                    cancellationToken);

        return assignments
            .OrderByDescending(
                assignment => assignment.AssignedDate)
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Update
    // ================================================================

    public async Task<AssetAssignmentResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetAssignmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Assignment ID is required.",
                nameof(id));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assignment is null)
        {
            throw new KeyNotFoundException(
                $"Asset assignment '{id}' was not found.");
        }

        assignment.Update(
            request.ExpectedReturnDate,
            request.AssignmentLocation,
            request.Remarks);

        _unitOfWork.AssetAssignments.Update(
            assignment);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assignment);
    }


    // ================================================================
    // Complete / Return
    // ================================================================

    public async Task<AssetAssignmentResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetAssignmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Assignment ID is required.",
                nameof(id));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assignment is null)
        {
            throw new KeyNotFoundException(
                $"Asset assignment '{id}' was not found.");
        }

        assignment.Complete(
            request.ActualReturnDate);

        _unitOfWork.AssetAssignments.Update(
            assignment);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assignment);
    }


    // ================================================================
    // Cancel
    // ================================================================

    public async Task<AssetAssignmentResponseDto> CancelAsync(
        Guid id,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Assignment ID is required.",
                nameof(id));
        }

        var assignment =
            await _unitOfWork.AssetAssignments
                .GetByIdAsync(
                    id,
                    cancellationToken);

        if (assignment is null)
        {
            throw new KeyNotFoundException(
                $"Asset assignment '{id}' was not found.");
        }

        assignment.Cancel(reason);

        _unitOfWork.AssetAssignments.Update(
            assignment);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(assignment);
    }


    // ================================================================
    // Assignment Number
    // ================================================================

    private async Task<string> GenerateAssignmentNumberAsync(
        CancellationToken cancellationToken)
    {
        var prefix =
            $"ASN-{DateTime.UtcNow:yyyyMMdd}";

        var assignments =
            await _unitOfWork.AssetAssignments
                .GetAllAsync(cancellationToken);

        var todayCount =
            assignments.Count(assignment =>
                assignment.AssignmentNumber
                    .StartsWith(prefix));

        return $"{prefix}-{todayCount + 1:D4}";
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static AssetAssignmentResponseDto MapToResponse(
        AssetAssignment assignment)
    {
        return new AssetAssignmentResponseDto
        {
            Id =
                assignment.Id,

            AssignmentNumber =
                assignment.AssignmentNumber,

            AssetId =
                assignment.AssetId,

            AssetRequestId =
                assignment.AssetRequestId,

            EmployeeId =
                assignment.EmployeeId,

            AssignedById =
                assignment.AssignedById,

            AssignedDate =
                assignment.AssignedDate,

            ExpectedReturnDate =
                assignment.ExpectedReturnDate,

            ActualReturnDate =
                assignment.ActualReturnDate,

            AssignmentLocation =
                assignment.AssignmentLocation,

            ConditionAtAssignment =
                assignment.ConditionAtAssignment,

            Remarks =
                assignment.Remarks,

            Status =
                assignment.Status,

            IsActive =
                assignment.IsActive,

        };
    }


    private async Task<AssetAssignmentDetailResponseDto>
        MapToDetailResponseAsync(
            AssetAssignment assignment,
            CancellationToken cancellationToken)
    {
        var response =
            new AssetAssignmentDetailResponseDto
            {
                Id =
                    assignment.Id,

                AssignmentNumber =
                    assignment.AssignmentNumber,

                AssetId =
                    assignment.AssetId,

                AssetRequestId =
                    assignment.AssetRequestId,

                EmployeeId =
                    assignment.EmployeeId,

                AssignedById =
                    assignment.AssignedById,

                AssignedDate =
                    assignment.AssignedDate,

                ExpectedReturnDate =
                    assignment.ExpectedReturnDate,

                ActualReturnDate =
                    assignment.ActualReturnDate,

                AssignmentLocation =
                    assignment.AssignmentLocation,

                ConditionAtAssignment =
                    assignment.ConditionAtAssignment,

                Remarks =
                    assignment.Remarks,

                Status =
                    assignment.Status,

                IsActive =
                    assignment.IsActive
            };

        var asset =
            await _unitOfWork.Assets.GetByIdAsync(
                assignment.AssetId,
                cancellationToken);

        if (asset is not null)
        {
            response.AssetTag =
                asset.AssetTag;

            response.AssetName =
                asset.Name;
        }

        var assetRequest =
            await _unitOfWork.AssetRequests.GetByIdAsync(
                assignment.AssetRequestId,
                cancellationToken);

        if (assetRequest is not null)
        {
            response.AssetRequestNumber =
                assetRequest.RequestNumber;
        }

        var employee =
            await _unitOfWork.Users.GetByIdAsync(
                assignment.EmployeeId,
                cancellationToken);

        if (employee is not null)
        {
            response.EmployeeName =
                employee.FullName;
        }

        var assignedBy =
            await _unitOfWork.Users.GetByIdAsync(
                assignment.AssignedById,
                cancellationToken);

        if (assignedBy is not null)
        {
            response.AssignedByName =
                assignedBy.FullName;
        }

        return response;
    }
}