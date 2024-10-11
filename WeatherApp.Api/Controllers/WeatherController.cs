using Microsoft.AspNetCore.Mvc;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.Api.Controllers;

[ApiController]
[Route("/api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    [HttpGet("get/{latitude:decimal}/{longitude:decimal}")]
    public async Task<IActionResult> Get(decimal latitude, decimal longitude, CancellationToken ct)
    {
        var result = await weatherService.GetWeatherAsync(latitude, longitude, ct);
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }
}