using UAMS.Application.DTOs.AssetTransfers.Requests;
using UAMS.Application.DTOs.AssetTransfers.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.AssetTransfers;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class AssetTransferService : IAssetTransferService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssetTransferService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    // ================================================================
    // Create
    // ================================================================

    public async Task<AssetTransferResponseDto> CreateAsync(
        CreateAssetTransferRequestDto request,
        Guid requestedById,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (requestedById == Guid.Empty)
            throw new ArgumentException(
                "Requested by user is required.",
                nameof(requestedById));

        var asset = await _unitOfWork.Assets.GetByIdAsync(
            request.AssetId,
            cancellationToken);

        if (asset is null)
            throw new KeyNotFoundException(
                $"Asset '{request.AssetId}' was not found.");

        var assignment = await _unitOfWork.AssetAssignments
            .GetByIdAsync(
                request.AssetAssignmentId,
                cancellationToken);

        if (assignment is null)
            throw new KeyNotFoundException(
                $"Asset assignment '{request.AssetAssignmentId}' was not found.");

        if (assignment.AssetId != request.AssetId)
            throw new InvalidOperationException(
                "The asset assignment does not belong to the specified asset.");

        var requestedBy = await _unitOfWork.Users.GetByIdAsync(
            requestedById,
            cancellationToken);

        if (requestedBy is null)
            throw new KeyNotFoundException(
                $"Requesting user '{requestedById}' was not found.");

        var toEmployee = await _unitOfWork.Users.GetByIdAsync(
            request.ToEmployeeId,
            cancellationToken);

        if (toEmployee is null)
            throw new KeyNotFoundException(
                $"Destination employee '{request.ToEmployeeId}' was not found.");

        var toDepartment = await _unitOfWork.Departments.GetByIdAsync(
            request.ToDepartmentId,
            cancellationToken);

        if (toDepartment is null)
            throw new KeyNotFoundException(
                $"Destination department '{request.ToDepartmentId}' was not found.");

        var activeTransfer = await _unitOfWork.AssetTransfers
            .GetByStatusAsync(
                AssetTransferStatus.PendingApproval,
                cancellationToken);

        if (activeTransfer.Any(x =>
                x.AssetId == request.AssetId &&
                x.AssetAssignmentId == request.AssetAssignmentId))
        {
            throw new InvalidOperationException(
                "A pending transfer already exists for this asset assignment.");
        }

        var transferNumber = await GenerateTransferNumberAsync(
            cancellationToken);

        /*
         * The source employee and source department come from
         * the current asset assignment.
         *
         * The exact source department property depends on the
         * User/Department entity design.
         */

        var transfer = AssetTransfer.Create(
            transferNumber,
            request.AssetId,
            request.AssetAssignmentId,
            requestedById,
            assignment.EmployeeId,
            request.ToEmployeeId,
            await GetEmployeeDepartmentIdAsync(
                assignment.EmployeeId,
                cancellationToken),
            request.ToDepartmentId,
            assignment.AssignmentLocation,
            request.ToLocation,
            request.Reason,
            request.Remarks);

        await _unitOfWork.AssetTransfers.AddAsync(
            transfer,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Get By Id
    // ================================================================

    public async Task<AssetTransferDetailResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        return transfer is null
            ? null
            : MapToDetailResponse(transfer);
    }


    // ================================================================
    // Get By Transfer Number
    // ================================================================

    public async Task<AssetTransferResponseDto?>
        GetByTransferNumberAsync(
            string transferNumber,
            CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(transferNumber);

        var transfer = await _unitOfWork.AssetTransfers
            .GetByTransferNumberAsync(
                transferNumber.Trim(),
                cancellationToken);

        return transfer is null
            ? null
            : MapToResponse(transfer);
    }


    // ================================================================
    // Get By Asset
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByAssetIdAsync(
            Guid assetId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByAssetIdAsync(
                assetId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Asset Assignment
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByAssetAssignmentIdAsync(
            Guid assetAssignmentId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByAssetAssignmentIdAsync(
                assetAssignmentId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Requested By
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByRequestedByIdAsync(
            Guid requestedById,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByRequestedByIdAsync(
                requestedById,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By From Employee
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByFromEmployeeIdAsync(
            Guid fromEmployeeId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByFromEmployeeIdAsync(
                fromEmployeeId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By To Employee
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByToEmployeeIdAsync(
            Guid toEmployeeId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByToEmployeeIdAsync(
                toEmployeeId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By From Department
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByFromDepartmentIdAsync(
            Guid fromDepartmentId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByFromDepartmentIdAsync(
                fromDepartmentId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By To Department
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByToDepartmentIdAsync(
            Guid toDepartmentId,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByToDepartmentIdAsync(
                toDepartmentId,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Status
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetByStatusAsync(
            AssetTransferStatus status,
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetByStatusAsync(
                status,
                cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Pending
    // ================================================================

    public async Task<IReadOnlyList<AssetTransferResponseDto>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.AssetTransfers
            .GetPendingAsync(cancellationToken);

        return transfers
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Update
    // ================================================================

    public async Task<AssetTransferResponseDto> UpdateAsync(
        Guid id,
        UpdateAssetTransferRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        if (transfer is null)
            throw new KeyNotFoundException(
                $"Asset transfer '{id}' was not found.");

        var employee = await _unitOfWork.Users.GetByIdAsync(
            request.ToEmployeeId,
            cancellationToken);

        if (employee is null)
            throw new KeyNotFoundException(
                $"Destination employee '{request.ToEmployeeId}' was not found.");

        var department = await _unitOfWork.Departments.GetByIdAsync(
            request.ToDepartmentId,
            cancellationToken);

        if (department is null)
            throw new KeyNotFoundException(
                $"Destination department '{request.ToDepartmentId}' was not found.");

        transfer.Update(
            request.ToEmployeeId,
            request.ToDepartmentId,
            request.ToLocation,
            request.Reason,
            request.Remarks);

        _unitOfWork.AssetTransfers.Update(transfer);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Approve
    // ================================================================

    public async Task<AssetTransferResponseDto> ApproveAsync(
        Guid id,
        ApproveAssetTransferRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        if (transfer is null)
            throw new KeyNotFoundException(
                $"Asset transfer '{id}' was not found.");

        var approver = await _unitOfWork.Users.GetByIdAsync(
            approvedById,
            cancellationToken);

        if (approver is null)
            throw new KeyNotFoundException(
                $"Approving user '{approvedById}' was not found.");

        transfer.Approve(
            approvedById,
            request.ApprovalRemarks);

        _unitOfWork.AssetTransfers.Update(transfer);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Reject
    // ================================================================

    public async Task<AssetTransferResponseDto> RejectAsync(
        Guid id,
        RejectAssetTransferRequestDto request,
        Guid approvedById,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        if (transfer is null)
            throw new KeyNotFoundException(
                $"Asset transfer '{id}' was not found.");

        var approver = await _unitOfWork.Users.GetByIdAsync(
            approvedById,
            cancellationToken);

        if (approver is null)
            throw new KeyNotFoundException(
                $"Approving user '{approvedById}' was not found.");

        transfer.Reject(
            approvedById,
            request.ApprovalRemarks);

        _unitOfWork.AssetTransfers.Update(transfer);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Complete
    // ================================================================

    public async Task<AssetTransferResponseDto> CompleteAsync(
        Guid id,
        CompleteAssetTransferRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        if (transfer is null)
            throw new KeyNotFoundException(
                $"Asset transfer '{id}' was not found.");

        transfer.Complete(request.Remarks);

        _unitOfWork.AssetTransfers.Update(transfer);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Cancel
    // ================================================================

    public async Task<AssetTransferResponseDto> CancelAsync(
        Guid id,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        var transfer = await _unitOfWork.AssetTransfers.GetByIdAsync(
            id,
            cancellationToken);

        if (transfer is null)
            throw new KeyNotFoundException(
                $"Asset transfer '{id}' was not found.");

        transfer.Cancel(reason);

        _unitOfWork.AssetTransfers.Update(transfer);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transfer);
    }


    // ================================================================
    // Generate Transfer Number
    // ================================================================

    private async Task<string> GenerateTransferNumberAsync(
        CancellationToken cancellationToken)
    {
        const string prefix = "TRF";

        var transferNumber =
            $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

        while (await _unitOfWork.AssetTransfers
                   .GetByTransferNumberAsync(
                       transferNumber,
                       cancellationToken) is not null)
        {
            transferNumber =
                $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Random.Shared.Next(100, 999)}";
        }

        return transferNumber;
    }


    // ================================================================
    // Get Employee Department
    // ================================================================

    private async Task<Guid> GetEmployeeDepartmentIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.Users.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is null)
            throw new KeyNotFoundException(
                $"Employee '{employeeId}' was not found.");

        /*
         * Replace this with the actual DepartmentId property
         * from your User entity.
         */
        return employee.DepartmentId;
    }


    // ================================================================
    // Response Mapping
    // ================================================================

    private static AssetTransferResponseDto MapToResponse(
        AssetTransfer transfer)
    {
        return new AssetTransferResponseDto
        {
            Id = transfer.Id,
            TransferNumber = transfer.TransferNumber,

            AssetId = transfer.AssetId,

            AssetTag = transfer.Asset?.AssetTag ?? string.Empty,
            AssetName = transfer.Asset?.Name ?? string.Empty,

            AssetAssignmentId = transfer.AssetAssignmentId,

            RequestedById = transfer.RequestedById,
            RequestedByName = transfer.RequestedBy?.FullName
                ?? string.Empty,

            FromEmployeeId = transfer.FromEmployeeId,
            FromEmployeeName = transfer.FromEmployee?.FullName
                ?? string.Empty,

            ToEmployeeId = transfer.ToEmployeeId,
            ToEmployeeName = transfer.ToEmployee?.FullName
                ?? string.Empty,

            FromDepartmentId = transfer.FromDepartmentId,
            FromDepartmentName = transfer.FromDepartment?.Name
                ?? string.Empty,

            ToDepartmentId = transfer.ToDepartmentId,
            ToDepartmentName = transfer.ToDepartment?.Name
                ?? string.Empty,

            FromLocation = transfer.FromLocation,
            ToLocation = transfer.ToLocation,

            Reason = transfer.Reason,
            RequestedDate = transfer.RequestedDate,

            ApprovedById = transfer.ApprovedById,

            ApprovedByName = transfer.ApprovedBy?.FullName,

            ApprovedDate = transfer.ApprovedDate,
            ApprovalRemarks = transfer.ApprovalRemarks,

            CompletedDate = transfer.CompletedDate,
            Remarks = transfer.Remarks,

            Status = transfer.Status,

            IsActive =
                transfer.Status == AssetTransferStatus.PendingApproval ||
                transfer.Status == AssetTransferStatus.Approved
        };
    }


    private static AssetTransferDetailResponseDto MapToDetailResponse(
        AssetTransfer transfer)
    {
        return new AssetTransferDetailResponseDto
        {
            Id = transfer.Id,
            TransferNumber = transfer.TransferNumber,

            AssetId = transfer.AssetId,
            AssetTag = transfer.Asset?.AssetTag ?? string.Empty,
            AssetName = transfer.Asset?.Name ?? string.Empty,

            AssetAssignmentId = transfer.AssetAssignmentId,
            AssignmentNumber =
                transfer.AssetAssignment?.AssignmentNumber,

            RequestedById = transfer.RequestedById,
            RequestedByName =
                transfer.RequestedBy?.FullName ?? string.Empty,

            RequestedDate = transfer.RequestedDate,

            FromEmployeeId = transfer.FromEmployeeId,
            FromEmployeeName =
                transfer.FromEmployee?.FullName ?? string.Empty,

            FromDepartmentId = transfer.FromDepartmentId,
            FromDepartmentName =
                transfer.FromDepartment?.Name ?? string.Empty,

            FromLocation = transfer.FromLocation,

            ToEmployeeId = transfer.ToEmployeeId,
            ToEmployeeName =
                transfer.ToEmployee?.FullName ?? string.Empty,

            ToDepartmentId = transfer.ToDepartmentId,
            ToDepartmentName =
                transfer.ToDepartment?.Name ?? string.Empty,

            ToLocation = transfer.ToLocation,

            Reason = transfer.Reason,
            Remarks = transfer.Remarks,

            ApprovedById = transfer.ApprovedById,
            ApprovedByName =
                transfer.ApprovedBy?.FullName,

            ApprovedDate = transfer.ApprovedDate,
            ApprovalRemarks = transfer.ApprovalRemarks,

            CompletedDate = transfer.CompletedDate,

            Status = transfer.Status,

            IsActive =
                transfer.Status == AssetTransferStatus.PendingApproval ||
                transfer.Status == AssetTransferStatus.Approved,

            CreatedAt = transfer.CreatedAt,
            UpdatedAt = transfer.UpdatedAt
        };
    }
}