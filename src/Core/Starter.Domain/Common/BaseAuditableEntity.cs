using Starter.Domain.Common.Contracts;

namespace Starter.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
{
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

    public string? ModifiedBy { get; set; }

    public DateTimeOffset DeletedOn { get; set; } = DateTimeOffset.UtcNow;

    public string? DeletedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
}

