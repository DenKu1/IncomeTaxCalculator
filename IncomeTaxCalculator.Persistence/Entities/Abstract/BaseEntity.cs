using System.ComponentModel.DataAnnotations;

namespace IncomeTaxCalculator.Persistence.Entities.Abstract;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
