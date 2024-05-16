using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using ProgramApi.Helpers.ConfigurationSettings;
using Microsoft.OpenApi.Models;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using ProgramApi.Helpers.Extensions;
using ProgramApi.Interfaces;
using ProgramApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOtherServices();//Used to separate concerns for custom added dependencies
builder.Services.AddLogging();
builder.Services.AddScoped<IQuestionModifier, QuestionModifier>();


// Configuration for config
builder.Configuration.AddJsonFile("appsettings.json");
ConfigurationSettingsHelper.Configuration = builder.Configuration;

// Use Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read Serilog settings from appsettings.json
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate, applyThemeToRedirectedOutput: true)
    .CreateLogger();
builder.Logging.AddSerilog();

//Configure dependency lifetimes
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCosmosDBServices();//Dependency Injection for employer and applicant services

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Capital Placement Collections",
        Version = "v1",
        Description = "Manage your Programs using Capital Placement Robust API"
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
