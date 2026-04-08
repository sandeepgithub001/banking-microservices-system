using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri(builder.Configuration["Consul:Address"]!);
}));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapGet("/services", async (IConsulClient consul) =>
{
    var services = await consul.Agent.Services();
    return Results.Ok(services.Response?.Values.Select(s => new { s.Service, s.Address, s.Port }));
});

app.MapControllers();
app.Run();