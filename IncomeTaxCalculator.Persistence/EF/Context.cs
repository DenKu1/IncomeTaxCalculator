using IncomeTaxCalculator.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.Persistence.EF;

public class IncomeTaxDbContext : DbContext
{
    public IncomeTaxDbContext(DbContextOptions<IncomeTaxDbContext> options) : base(options)
    {
    }

    public DbSet<TaxBand> TaxBands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxBand>().HasData([
            new()
            {
                Id = Guid.NewGuid(),
                AnnualSalaryLowerLimit = 0,
                AnnualSalaryUpperLimit = 5000,
                TaxRate = 0
            },
            new()
            {
                Id = Guid.NewGuid(),
                AnnualSalaryLowerLimit = 5000,
                AnnualSalaryUpperLimit = 20000,
                TaxRate = 20
            },
            new()
            {
                Id = Guid.NewGuid(),
                AnnualSalaryLowerLimit = 20000,
                TaxRate = 40
            }
        ]);

        base.OnModelCreating(modelBuilder);
    }
}
