using MassTransit;
using Microsoft.Extensions.Options;
using SupPortal.Shared;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Extension;

public static class BusConfigurator
{
    private static MqSettingsDto _settings;
 

    public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> configureBus = null)
    {
        var configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();

        _settings = configuration.GetSection("RabbitMQConfig").Get<MqSettingsDto>();

        return Bus.Factory.CreateUsingRabbitMq(ft =>
        {
            ft.Host(_settings.Host, cfg =>
            {
                cfg.Username(_settings.Username);
                cfg.Password(_settings.Password);
            });
            configureBus?.Invoke(ft);
        });

    }
}
