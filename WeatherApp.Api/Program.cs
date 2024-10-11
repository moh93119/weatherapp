using Microsoft.Extensions.Http.Resilience;
using Polly;
using WeatherApp.Application.Options;
using WeatherApp.Application.Services;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<OpenMeteoSetting>().Bind(builder.Configuration.GetSection(nameof(OpenMeteoSetting)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("MyHttpClient")
    .AddResilienceHandler("WeatherApiResilienceStrategy", (handlerBuilder, _) =>
    {
        handlerBuilder
            .AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                UseJitter = true,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(1)
            })
            .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(5),
                MinimumThroughput = 5, 
                BreakDuration = TimeSpan.FromSeconds(30)
            });
    });;

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();