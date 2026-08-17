using FriendsDebt.Domain.Enums;

namespace FriendsDebt.Domain.Common;

public abstract class AuditableEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string CreatedBy { get; private set; } = string.Empty;

    public DateTime Created { get; private set; }

    public string ModifiedBy { get; private set; } = string.Empty;

    public DateTime Modified { get; private set; }

    public AuditableEntityStatus Status { get; private set; }

    public string InactivatedBy { get; private set; } = string.Empty;

    public DateTime? Inactivated { get; private set; }
    
    public void SetCreated(string userEmail, DateTime created)
    {
        CreatedBy = userEmail;
        Created = created;
        Status = AuditableEntityStatus.Active;
    }

    public void SetModified(string userEmail, DateTime modified)
    {
        ModifiedBy = userEmail;
        Modified = modified;
    }
    
    public void SetInactivated(string userEmail, DateTime inactivated)
    {
        ModifiedBy = userEmail;
        Modified = inactivated;
        InactivatedBy = userEmail;
        Inactivated = inactivated;
        Status = AuditableEntityStatus.Inactive;
    }
}
