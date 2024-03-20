using FluentValidation.AspNetCore;
using IncomeTaxCalculator.API.Startup;
using IncomeTaxCalculator.Persistence.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLogging();

builder.Services.AddDbContext<IncomeTaxDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<ApiBehaviorOptions>(a =>
{
    a.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();

builder.RegisterMapper();
builder.RegisterValidators();
builder.RegisterDependencies();

builder.Services.AddSwaggerGen(x => x.EnableAnnotations());

var app = builder.Build();

app.RegisterMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Income Tax API");
    }
});

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
