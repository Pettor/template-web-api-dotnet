namespace Backend.Domain.Common.Contracts;

public interface IAuditableEntity
{
    Guid CreatedBy { get; set; }

    DateTime CreatedOn { get; }

    Guid LastModifiedBy { get; set; }

    DateTime? LastModifiedOn { get; set; }
}
