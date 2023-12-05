namespace Starter.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

    public string? ModifiedBy { get; set; }
}

