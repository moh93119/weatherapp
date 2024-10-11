using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Interfaces;

public interface IWeatherService
{
    Task<WeatherData?> GetWeatherAsync(decimal latitude, decimal longitude, CancellationToken ct);
}