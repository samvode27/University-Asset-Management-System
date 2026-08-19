using UAMS.Domain.Enums;

namespace UAMS.Application.DTOs.AssetAssignments.Responses;

public class AssetAssignmentDetailResponseDto
{
    public Guid Id { get; set; }

    public string AssignmentNumber { get; set; } = null!;

    // ============================================================
    // Asset
    // ============================================================

    public Guid AssetId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    // ============================================================
    // Asset Request
    // ============================================================

    public Guid AssetRequestId { get; set; }

    public string AssetRequestNumber { get; set; } = null!;

    // ============================================================
    // Employee
    // ============================================================

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    // ============================================================
    // Assigned By
    // ============================================================

    public Guid AssignedById { get; set; }

    public string AssignedByName { get; set; } = null!;

    // ============================================================
    // Assignment
    // ============================================================

    public DateTime AssignedDate { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public string? AssignmentLocation { get; set; }

    public AssetCondition ConditionAtAssignment { get; set; }

    public string? Remarks { get; set; }

    public AssetAssignmentStatus Status { get; set; }

    public bool IsActive { get; set; }
}