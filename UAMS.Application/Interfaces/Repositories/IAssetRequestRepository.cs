using UAMS.Domain.Entities.AssetRequests;
using UAMS.Domain.Enums;

namespace UAMS.Application.Interfaces.Repositories;

public interface IAssetRequestRepository : IRepository<AssetRequest>
{
    Task<AssetRequest?> GetByRequestNumberAsync(
        string requestNumber,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetRequest>> GetByRequesterIdAsync(
        Guid requesterId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetRequest>> GetByAssetIdAsync(
        Guid assetId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetRequest>> GetByDepartmentIdAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetRequest>> GetByStatusAsync(
        AssetRequestStatus status,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetRequest>> GetByRequesterAndStatusAsync(
        Guid requesterId,
        AssetRequestStatus status,
        CancellationToken cancellationToken = default);
}