using UAMS.Application.DTOs.DamageReports.Requests;
using UAMS.Application.DTOs.DamageReports.Responses;

namespace UAMS.Application.Interfaces.Services;

public interface IDamageReportService
{
    Task<DamageReportResponseDto?> CreateDamageReportAsync(
        Guid reportedById,
        CreateDamageReportRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> GetDamageReportAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DamageReportDetailResponseDto?> GetDamageReportDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DamageReportListResponseDto> GetDamageReportsAsync(
        DamageReportFilterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> UpdateDamageReportAsync(
        Guid id,
        UpdateDamageReportRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> StartReviewAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> AssessDamageReportAsync(
        Guid id,
        Guid assessedById,
        AssessDamageReportRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> ResolveDamageReportAsync(
        Guid id,
        ResolveDamageReportRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> RejectDamageReportAsync(
        Guid id,
        Guid assessedById,
        RejectDamageReportRequestDto request,
        CancellationToken cancellationToken = default);

    Task<DamageReportResponseDto?> CancelDamageReportAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}