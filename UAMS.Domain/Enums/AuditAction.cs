namespace UAMS.Domain.Enums;

public enum AuditAction
{
    Create,
    Update,
    Delete,
    Restore,

    Activate,
    Deactivate,

    Approve,
    Reject,
    Submit,
    Cancel,

    Assign,
    Transfer,
    Return,

    Generate,
    Login,
    Logout,

    PasswordChange,
    PasswordReset,

    Export,
    Import,

    Dispose,
    Maintenance,

    Other
}