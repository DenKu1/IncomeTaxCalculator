using IncomeTaxCalculator.Persistence.EF;
using IncomeTaxCalculator.Persistence.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IncomeTaxCalculator.API.IntegrationTests.Utilities;

public class IncomeTaxCalculatorWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.SingleOrDefault(descriptor => descriptor
                .ServiceType == typeof(DbContextOptions<IncomeTaxDbContext>)));

            var serviceProvider = GetInMemoryServiceProvider();

            services.AddDbContextPool<IncomeTaxDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.Empty.ToString());
                options.UseInternalServiceProvider(serviceProvider);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IncomeTaxDbContext>();

            SeedData(context);
        });
    }

    private void SeedData(IncomeTaxDbContext context)
    {
        var taxBand1 = new TaxBand()
        {
            Id = Guid.NewGuid(),
            AnnualSalaryLowerLimit = 0,
            AnnualSalaryUpperLimit = 5000,
            TaxRate = 0
        };

        var taxBand2 = new TaxBand()
        {
            Id = Guid.NewGuid(),
            AnnualSalaryLowerLimit = 5000,
            AnnualSalaryUpperLimit = 20000,
            TaxRate = 20
        };

        var taxBand3 = new TaxBand()
        {
            Id = Guid.NewGuid(),
            AnnualSalaryLowerLimit = 20000,
            TaxRate = 40
        };

        context.AddRange(new TaxBand[] { taxBand1, taxBand2, taxBand3 });

        context.SaveChanges();
    }

    private static ServiceProvider GetInMemoryServiceProvider()
    {
        return new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();
    }
}