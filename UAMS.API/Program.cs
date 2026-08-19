using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UAMS.Application;
using UAMS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ================================================================
// Database
// ================================================================

builder.Services.AddDbContext<UAMSDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UAMSConnection"));
});

// ================================================================
// Controllers
// ================================================================

builder.Services.AddControllers();

// ================================================================
// API Documentation
// ================================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// ================================================================
// Validators
// ================================================================

builder.Services.AddValidatorsFromAssembly(
    typeof(AssemblyReference).Assembly);

// ================================================================
// HTTP Request Pipeline
// ================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();