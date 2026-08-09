namespace UAMS.Domain.Enums;

public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    Restore = 4,

    Activate = 5,
    Deactivate = 6,

    Approve = 7,
    Reject = 8,
    Submit = 9,
    Cancel = 10,

    Assign = 11,
    Transfer = 12,
    Return = 13,

    Generate = 14,
    Login = 15,
    Logout = 16,

    PasswordChange = 17,
    PasswordReset = 18,

    Export = 19,
    Import = 20,

    Dispose = 21,
    Maintenance = 22,

    Other = 99
}