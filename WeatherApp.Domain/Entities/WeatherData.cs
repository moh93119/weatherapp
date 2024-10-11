namespace WeatherApp.Domain.Entities;

public class WeatherData
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public required string Timezone { get; set; }
    
    public required DateTime CreateDate { get; set; }

    public HourlyModel Hourly { get; set; }
    
    public class HourlyModel
    {
        public List<string> Times { get; set; } = [];

        public List<double> Temperature2Ms { get; set; } = [];
    }
}