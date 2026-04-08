using Microsoft.AspNetCore.Mvc;

namespace MicroBank.ConfigService.Controllers;

[ApiController]
[Route("config")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ConfigController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("{serviceName}")]
    public IActionResult GetServiceConfig(string serviceName, [FromQuery] string environment = "Development")
    {
        var root = _configuration.GetSection(environment);
        if (!root.Exists())
        {
            return NotFound(new { error = $"Environment '{environment}' not found." });
        }

        var service = root.GetSection(serviceName);
        if (!service.Exists())
        {
            return NotFound(new { error = $"Configuration for service '{serviceName}' not found in '{environment}'." });
        }

        return Ok(service.GetChildren().ToDictionary(x => x.Key, x => x.Value));
    }
}
