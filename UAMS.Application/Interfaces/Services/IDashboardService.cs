using UAMS.Application.DTOs.Dashboard.Requests;
using UAMS.Application.DTOs.Dashboard.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardResponseDto> GetDashboardAsync(
        DashboardFilterRequestDto request,
        CancellationToken cancellationToken = default);
}