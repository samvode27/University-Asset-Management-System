using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.Reservations.Responses;

public class ReservationResponseDto
{
    public Guid Id { get; set; }

    public string ReservationNumber { get; set; } = null!;


    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string AssetNumber { get; set; } = null!;

    public string AssetName { get; set; } = null!;


    // ============================================================
    // Employee
    // ============================================================

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeNumber { get; set; } = null!;


    // ============================================================
    // Department
    // ============================================================

    public Guid DepartmentId { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;


    // ============================================================
    // Reservation Period
    // ============================================================

    public DateTime ReservationStartDate { get; set; }

    public DateTime ReservationEndDate { get; set; }


    // ============================================================
    // Purpose
    // ============================================================

    public string? Purpose { get; set; }


    // ============================================================
    // Approval
    // ============================================================

    public Guid? ApprovedById { get; set; }

    public string? ApprovedByName { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovalRemarks { get; set; }


    // ============================================================
    // Cancellation / Rejection
    // ============================================================

    public string? RejectionReason { get; set; }

    public string? CancellationReason { get; set; }


    // ============================================================
    // Completion
    // ============================================================

    public DateTime? CompletedDate { get; set; }


    // ============================================================
    // Status
    // ============================================================

    public ReservationStatus Status { get; set; }

    public string? Remarks { get; set; }


    // ============================================================
    // Audit / Active Information
    // ============================================================

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}