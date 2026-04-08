using Consul;

namespace MicroBank.AccountService.Services;

public static class ConsulRegistration
{
    public static async Task RegisterAsync(IHostApplicationLifetime lifetime, IConfiguration configuration)
    {
        var consulAddress = configuration["Consul:Address"] ?? "http://localhost:8500";
        var serviceName = configuration["Consul:ServiceName"] ?? "MicroBank.AccountService";
        var serviceId = configuration["Consul:ServiceId"] ?? $"{serviceName}-{Guid.NewGuid()}";
        var serviceAddress = configuration["Consul:ServiceAddress"] ?? "localhost";
        var servicePort = int.Parse(configuration["Consul:ServicePort"] ?? "6001");

        using var client = new ConsulClient(cfg => cfg.Address = new Uri(consulAddress));
        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = serviceName,
            Address = serviceAddress,
            Port = servicePort,
            Check = new AgentServiceCheck
            {
                HTTP = $"http://{serviceAddress}:{servicePort}/health",
                Interval = TimeSpan.FromSeconds(10),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
            }
        };

        await client.Agent.ServiceRegister(registration);
        lifetime.ApplicationStopping.Register(() => client.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult());
    }
}
