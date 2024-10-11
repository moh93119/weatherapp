using System.Text.Json.Serialization;

namespace WeatherApp.Application.Models;

public class OpenMeteoForecastResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    public required string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public required string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("hourly_units")]
    public HourlyUnitsData HourlyUnits { get; set; }

    [JsonPropertyName("hourly")]
    public HourlyData Hourly { get; set; }
    
    public class HourlyData
    {
        [JsonPropertyName("time")] public List<string> Time { get; set; } = [];

        [JsonPropertyName("temperature_2m")] public List<double> Temperature2M { get; set; } = [];
    }

    public class HourlyUnitsData
    {
        [JsonPropertyName("time")]
        public required string Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public required string Temperature2M { get; set; }
    }
}