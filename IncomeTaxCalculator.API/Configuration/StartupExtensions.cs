using IncomeTaxCalculator.API.Middleware;
using IncomeTaxCalculator.API.Startup;
using IncomeTaxCalculator.Domain.Services;
using IncomeTaxCalculator.Domain.Services.Interfaces;
using IncomeTaxCalculator.Persistence.Repositories;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IncomeTaxCalculator.API.Startup;
public static class StartupExtensions
{
    public static void RegisterMapper(this WebApplicationBuilder builder, Assembly currentAssembly)
    {
        builder.Services.AddAutoMapper(currentAssembly);
    }

    public static void AddSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
    }

    public static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static void UseCors(this WebApplication app)
    {
        app.UseCors(builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    }

    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IIncomeTaxService, IncomeTaxService>();
        builder.Services.AddScoped<ITaxBandService, TaxBandService>();

        builder.Services.AddScoped<ITaxBandRepository, TaxBandRepository>();
    }

    public static void ApplyMigration<TContext>(this WebApplication app) where TContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        var _db = scope.ServiceProvider.GetRequiredService<TContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
            _db.Database.Migrate();
    }
}
