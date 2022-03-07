using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TrailRunning.Races.Core.Repository;
using TrailRunning.Races.Core.Serialization;
using TrailRunning.Races.Management.Host.Configuration;
using TrailRunning.Races.Management.Host.Features.Races;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
    .AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddCustomProblemDetails();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyConverter());
});

builder.Services.AddScoped(typeof(IMartenRepository<>), typeof(MartenRepository<>));
builder.Services.AddCustomMarten(options => options.ConfigureRaces());

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();

app.MapRaces();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
