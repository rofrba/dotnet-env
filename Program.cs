using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var myService = services.GetRequiredService<MyService>();
            myService.PrintConfig();
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();

                // Agrega configuración desde appsettings.json
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                // Sobrescribe la configuración con variables de entorno
                config.AddEnvironmentVariables(prefix: "MYAPP_");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<MyService>();
            })
            .ConfigureLogging(logging =>
            {
                Console.WriteLine("Configuring logging...");
                logging.ClearProviders();
                logging.AddConsole();
            });
}

public class MyService
{
    private readonly IConfiguration _configuration;

    public MyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void PrintConfig()
    {
        var mySettingValue = _configuration["AppSettings:MySetting"];
        Console.WriteLine($"Value from appsettings.json: {mySettingValue}");

        // Accede a las variables de entorno directamente
        var mySecretEnv = Environment.GetEnvironmentVariable("MYAPP_MY_VARIABLE");
        Console.WriteLine($"Secret from environment variable: {mySecretEnv}");

    }
}
