using FluentValidation;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Domain.Contracts.Interfaces;
using URLShortener.Domain.Contracts.Models.RequestModels;
using URLShortener.Domain.Services;
using URLShortener.Manager;
using URLShortener.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddScoped<IAppManager, AppManager>();
services.AddScoped<IAppService, AppService>();
services.AddScoped<IUrlService, UrlService>();
services.AddScoped<IValidator<CreateUrlRequestModel>, CreateUrlRequestModelValidator>();
services.AddScoped<IValidator<CreateUserRequestModel>, CreateUserRequestModelValidator>();
services.AddHostedService<CleanupService>();

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var logger = app.Logger;
logger.LogInformation("Application starting up");

app.Run();