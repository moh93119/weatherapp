using Microsoft.Extensions.Configuration;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace WeatherApp.Infrastructure.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _connectionString;

    public WeatherRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WeatherDB")!;
    }
    public async Task<WeatherData?> GetLastWeatherDataAsync(decimal latitude, decimal longitude, CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<WeatherData>(
            "SELECT TOP 1 * FROM Weathers WHERE Latitude = {latitude} AND Longitude = {longitude} ORDER BY CreateDate DESC"
        );
    }

    public async Task SaveWeatherDataAsync(WeatherData data, CancellationToken ct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "INSERT INTO Weathers (Data) VALUES (@Data)",
            data
        );
    }
}