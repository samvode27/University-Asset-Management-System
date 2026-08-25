using UAMS.Application.DTOs.DamageReports.Requests;
using UAMS.Application.DTOs.DamageReports.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.DamageReports;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services;

public class DamageReportService : IDamageReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public DamageReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // ============================================================
    // Create Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        CreateDamageReportAsync(
            Guid reportedById,
            CreateDamageReportRequestDto request,
            CancellationToken cancellationToken = default)
    {
        if (reportedById == Guid.Empty)
        {
            throw new ArgumentException(
                "Reported by user ID is required.",
                nameof(reportedById));
        }

        var asset =
            await _unitOfWork.Assets.GetByIdAsync(
                request.AssetId,
                cancellationToken);

        if (asset is null || asset.IsDeleted)
        {
            return null;
        }

        var assignment =
            await _unitOfWork.AssetAssignments.GetByIdAsync(
                request.AssetAssignmentId,
                cancellationToken);

        if (assignment is null || assignment.IsDeleted)
        {
            return null;
        }

        var reportedBy =
            await _unitOfWork.Users.GetByIdAsync(
                reportedById,
                cancellationToken);

        if (reportedBy is null || reportedBy.IsDeleted)
        {
            return null;
        }

        var reportNumber =
            await GenerateReportNumberAsync(
                cancellationToken);

        var report =
            DamageReport.Create(
                reportNumber,
                request.AssetId,
                request.AssetAssignmentId,
                reportedById,
                request.DamageType,
                request.Severity,
                request.Description,
                request.IncidentDate,
                request.IncidentLocation,
                request.Remarks);

        await _unitOfWork.DamageReports.AddAsync(
            report,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Get Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        GetDamageReportAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted)
        {
            return null;
        }

        return MapResponse(report);
    }


    // ============================================================
    // Get Damage Report Details
    // ============================================================

    public async Task<DamageReportDetailResponseDto?>
        GetDamageReportDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted)
        {
            return null;
        }

        return MapDetailResponse(report);
    }


    // ============================================================
    // Get Damage Reports
    // ============================================================

    public async Task<DamageReportListResponseDto>
        GetDamageReportsAsync(
            DamageReportFilterRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var reports =
            await _unitOfWork.DamageReports.FindAsync(
                report =>
                    !report.IsDeleted &&
                    (!request.AssetId.HasValue ||
                     report.AssetId == request.AssetId.Value) &&

                    (!request.AssetAssignmentId.HasValue ||
                     report.AssetAssignmentId ==
                     request.AssetAssignmentId.Value) &&

                    (!request.ReportedById.HasValue ||
                     report.ReportedById ==
                     request.ReportedById.Value) &&

                    (!request.AssessedById.HasValue ||
                     report.AssessedById ==
                     request.AssessedById.Value) &&

                    (!request.DamageType.HasValue ||
                     report.DamageType ==
                     request.DamageType.Value) &&

                    (!request.Severity.HasValue ||
                     report.Severity ==
                     request.Severity.Value) &&

                    (!request.Status.HasValue ||
                     report.Status ==
                     request.Status.Value) &&

                    (!request.IsRepairable.HasValue ||
                     report.IsRepairable ==
                     request.IsRepairable.Value) &&

                    (!request.ReportedFromDate.HasValue ||
                     report.ReportedDate >=
                     request.ReportedFromDate.Value) &&

                    (!request.ReportedToDate.HasValue ||
                     report.ReportedDate <=
                     request.ReportedToDate.Value) &&

                    (string.IsNullOrWhiteSpace(
                         request.ReportNumber) ||
                     report.ReportNumber.Contains(
                         request.ReportNumber)) &&

                    (string.IsNullOrWhiteSpace(
                         request.SearchTerm) ||
                     report.ReportNumber.Contains(
                         request.SearchTerm) ||
                     report.Description.Contains(
                         request.SearchTerm) ||
                     (report.IncidentLocation != null &&
                      report.IncidentLocation.Contains(
                          request.SearchTerm))),
                cancellationToken);

        var orderedReports =
            reports
                .OrderByDescending(
                    x => x.ReportedDate)
                .ToList();

        var totalCount =
            orderedReports.Count;

        var totalPages =
            totalCount == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount /
                    (double)request.PageSize);

        var items =
            orderedReports
                .Skip(
                    (request.PageNumber - 1) *
                    request.PageSize)
                .Take(request.PageSize)
                .Select(MapResponse)
                .ToList();

        return new DamageReportListResponseDto
        {
            Items = items,

            PageNumber = request.PageNumber,

            PageSize = request.PageSize,

            TotalCount = totalCount,

            TotalPages = totalPages,

            HasPreviousPage =
                request.PageNumber > 1,

            HasNextPage =
                request.PageNumber < totalPages
        };
    }


    // ============================================================
    // Update Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        UpdateDamageReportAsync(
            Guid id,
            UpdateDamageReportRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted ||
            !report.IsActive)
        {
            return null;
        }

        if (report.Status != DamageReportStatus.Submitted)
        {
            throw new InvalidOperationException(
                "Only submitted damage reports can be updated.");
        }

        report.Update(
            request.DamageType,
            request.Severity,
            request.Description,
            request.IncidentDate,
            request.IncidentLocation,
            request.Remarks);

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Start Review
    // ============================================================

    public async Task<DamageReportResponseDto?>
        StartReviewAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted ||
            !report.IsActive)
        {
            return null;
        }

        if (report.Status != DamageReportStatus.Submitted)
        {
            throw new InvalidOperationException(
                "Only submitted damage reports can be placed under review.");
        }

        report.StartReview();

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Assess Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        AssessDamageReportAsync(
            Guid id,
            Guid assessedById,
            AssessDamageReportRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted ||
            !report.IsActive)
        {
            return null;
        }

        var assessor =
            await _unitOfWork.Users.GetByIdAsync(
                assessedById,
                cancellationToken);

        if (assessor is null ||
            assessor.IsDeleted)
        {
            return null;
        }

        if (report.Status !=
            DamageReportStatus.UnderReview)
        {
            throw new InvalidOperationException(
                "Only damage reports under review can be assessed.");
        }

        if (request.IsRepairable)
        {
            report.MarkMaintenanceRequired(
                assessedById,
                request.Assessment);
        }
        else
        {
            report.MarkUnrepairable(
                assessedById,
                request.Assessment);
        }

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Resolve Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        ResolveDamageReportAsync(
            Guid id,
            ResolveDamageReportRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted)
        {
            return null;
        }

        if (report.Status !=
            DamageReportStatus.MaintenanceRequired)
        {
            throw new InvalidOperationException(
                "Only damage reports requiring maintenance can be resolved.");
        }

        report.Resolve(
            request.ResolutionRemarks);

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Reject Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        RejectDamageReportAsync(
            Guid id,
            Guid assessedById,
            RejectDamageReportRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted ||
            !report.IsActive)
        {
            return null;
        }

        var assessor =
            await _unitOfWork.Users.GetByIdAsync(
                assessedById,
                cancellationToken);

        if (assessor is null ||
            assessor.IsDeleted)
        {
            return null;
        }

        if (report.Status !=
            DamageReportStatus.UnderReview)
        {
            throw new InvalidOperationException(
                "Only damage reports under review can be rejected.");
        }

        report.Reject(
            assessedById,
            request.RejectionReason);

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Cancel Damage Report
    // ============================================================

    public async Task<DamageReportResponseDto?>
        CancelDamageReportAsync(
            Guid id,
            CancellationToken cancellationToken = default)
    {
        var report =
            await _unitOfWork.DamageReports.GetByIdAsync(
                id,
                cancellationToken);

        if (report is null ||
            report.IsDeleted ||
            !report.IsActive)
        {
            return null;
        }

        if (report.Status !=
            DamageReportStatus.Submitted)
        {
            throw new InvalidOperationException(
                "Only submitted damage reports can be cancelled.");
        }

        report.Cancel();

        _unitOfWork.DamageReports.Update(report);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapResponse(report);
    }


    // ============================================================
    // Generate Report Number
    // ============================================================

    private async Task<string>
        GenerateReportNumberAsync(
            CancellationToken cancellationToken)
    {
        string reportNumber;

        do
        {
            reportNumber =
                $"DR-{DateTime.UtcNow:yyyyMMddHHmmssfff}" +
                $"-{Random.Shared.Next(100, 999)}";

        }
        while (
            await _unitOfWork.DamageReports
                .ExistsAsync(
                    x => x.ReportNumber == reportNumber,
                    cancellationToken));

        return reportNumber;
    }


    // ============================================================
    // Map Response
    // ============================================================

    private static DamageReportResponseDto
        MapResponse(
            DamageReport report)
    {
        return new DamageReportResponseDto
        {
            Id = report.Id,

            ReportNumber =
                report.ReportNumber,

            AssetId =
                report.AssetId,

            AssetAssignmentId =
                report.AssetAssignmentId,

            ReportedById =
                report.ReportedById,

            ReportedDate =
                report.ReportedDate,

            DamageType =
                report.DamageType,

            Severity =
                report.Severity,

            Description =
                report.Description,

            IncidentDate =
                report.IncidentDate,

            IncidentLocation =
                report.IncidentLocation,

            IsRepairable =
                report.IsRepairable,

            Assessment =
                report.Assessment,

            AssessedById =
                report.AssessedById,

            AssessedDate =
                report.AssessedDate,

            Status =
                report.Status,

            ResolutionRemarks =
                report.ResolutionRemarks,

            ResolvedDate =
                report.ResolvedDate,

            Remarks =
                report.Remarks,

            IsActive =
                report.IsActive,

            CreatedAt =
                report.CreatedAt
        };
    }


    // ============================================================
    // Map Detail Response
    // ============================================================

    private static DamageReportDetailResponseDto
        MapDetailResponse(
            DamageReport report)
    {
        return new DamageReportDetailResponseDto
        {
            Id = report.Id,

            ReportNumber =
                report.ReportNumber,

            AssetId =
                report.AssetId,

            AssetAssignmentId =
                report.AssetAssignmentId,

            ReportedById =
                report.ReportedById,

            ReportedDate =
                report.ReportedDate,

            DamageType =
                report.DamageType,

            Severity =
                report.Severity,

            Description =
                report.Description,

            IncidentDate =
                report.IncidentDate,

            IncidentLocation =
                report.IncidentLocation,

            IsRepairable =
                report.IsRepairable,

            Assessment =
                report.Assessment,

            AssessedById =
                report.AssessedById,

            AssessedDate =
                report.AssessedDate,

            Status =
                report.Status,

            ResolutionRemarks =
                report.ResolutionRemarks,

            ResolvedDate =
                report.ResolvedDate,

            Remarks =
                report.Remarks,

            IsActive =
                report.IsActive,

            CreatedAt =
                report.CreatedAt,

            UpdatedAt =
                report.UpdatedAt
        };
    }
}