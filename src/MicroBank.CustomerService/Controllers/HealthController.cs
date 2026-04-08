using Microsoft.AspNetCore.Mvc;

namespace MicroBank.CustomerService.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy" });
}
