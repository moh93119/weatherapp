using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Interfaces;

public interface IWeatherRepository
{
    Task<WeatherData?> GetLastWeatherDataAsync(decimal latitude, decimal longitude, CancellationToken ct);
    Task SaveWeatherDataAsync(WeatherData data, CancellationToken ct);
}