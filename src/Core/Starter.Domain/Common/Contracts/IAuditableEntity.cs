namespace Starter.Domain.Common.Contracts;
public interface IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}
