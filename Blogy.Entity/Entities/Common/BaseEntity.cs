namespace Blogy.Entity.Entities.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}