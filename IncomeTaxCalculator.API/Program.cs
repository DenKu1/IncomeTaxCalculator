using IncomeTaxCalculator.API.Startup;
using IncomeTaxCalculator.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddDbContext<IncomeTaxDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.RegisterMapper(assembly);
builder.RegisterDependencies();
builder.AddSwaggerGen();

var app = builder.Build();

app.RegisterMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Income Tax API");
        //c.RoutePrefix = "/";
    }
});

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();
app.ApplyMigration<IncomeTaxDbContext>();

app.Run();
