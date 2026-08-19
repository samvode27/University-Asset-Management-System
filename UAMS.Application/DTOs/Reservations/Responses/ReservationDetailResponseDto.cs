using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Reservations.Responses;

public class ReservationDetailResponseDto
{
    public Guid Id { get; set; }

    public string ReservationNumber { get; set; } = null!;


    // ============================================================
    // Asset Information
    // ============================================================

    public Guid AssetId { get; set; }

    public string AssetNumber { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetCategoryName { get; set; }

    public string? SerialNumber { get; set; }


    // ============================================================
    // Employee Information
    // ============================================================

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeIdNumber { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;


    // ============================================================
    // Reservation
    // ============================================================

    public DateTime ReservationStartDate { get; set; }

    public DateTime ReservationEndDate { get; set; }

    public string? Purpose { get; set; }


    // ============================================================
    // Approval
    // ============================================================

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovalRemarks { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public ReservationStatus Status { get; set; }

    public string? RejectionReason { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CompletedDate { get; set; }


    // ============================================================
    // Audit
    // ============================================================

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public string? Remarks { get; set; }
}