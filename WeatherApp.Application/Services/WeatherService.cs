using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherApp.Application.Models;
using WeatherApp.Application.Options;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Application.Services;

public class WeatherService(
    IOptions<OpenMeteoSetting> openMeteoSetting,
    IHttpClientFactory httpClientFactory,
    IWeatherRepository weatherRepository) : IWeatherService
{
    private readonly OpenMeteoSetting _openMeteoSetting = openMeteoSetting.Value;

    public async Task<WeatherData?> GetWeatherAsync(decimal latitude, decimal longitude, CancellationToken ct)
    {
        var uri = $"{_openMeteoSetting.BaseUrl}?latitude={latitude}&longitude={longitude}&hourly=temperature_2m";
        var client = httpClientFactory.CreateClient("MyHttpClient");

        WeatherData? weatherData = null;
        try
        {
            var httpResponse = await client.GetAsync(uri, ct);

            var result = await httpResponse.Content.ReadAsStringAsync(ct);

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<OpenMeteoForecastResponse>(result);
                if (response is not null)
                {
                    weatherData = new WeatherData
                    {
                        Latitude = response.Latitude,
                        Longitude = response.Longitude,
                        Timezone = response.Timezone,
                        CreateDate = DateTime.Now,
                        Hourly = new WeatherData.HourlyModel
                        {
                            Times = response.Hourly.Time,
                            Temperature2Ms = response.Hourly.Temperature2M
                        }
                    };
                    await weatherRepository.SaveWeatherDataAsync(weatherData, ct);
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        if (weatherData is not null) return weatherData;
        var latestData = await weatherRepository.GetLastWeatherDataAsync(latitude, longitude, ct);
        weatherData = latestData;

        return weatherData;
    }
}