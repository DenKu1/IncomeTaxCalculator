﻿using FluentValidation;
using IncomeTaxCalculator.API.Middleware;
using IncomeTaxCalculator.API.Profiles;
using IncomeTaxCalculator.API.Startup;
using IncomeTaxCalculator.API.Validators;
using IncomeTaxCalculator.Domain.Profiles;
using IncomeTaxCalculator.Domain.Services;
using IncomeTaxCalculator.Domain.Services.Interfaces;
using IncomeTaxCalculator.Persistence.Repositories;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.API.Startup;

public static class StartupExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(); 
    }

    public static void RegisterMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(IncomeTaxProfile), typeof(TaxBandDomainModelProfile));
    }

    public static void AddSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
    }

    public static void RegisterValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CalculateTaxRequestViewModelValidator>();
    }

    public static void RegisterMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();
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

        builder.Services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
    }

    public static void ApplyMigration<TContext>(this WebApplication app) where TContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        var _db = scope.ServiceProvider.GetRequiredService<TContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
            _db.Database.Migrate();
    }
}
