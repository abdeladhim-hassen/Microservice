using MassTransit;
using Ordering.API.EventBusConsumer;

namespace Basket.API.Extensions
{
    public static class RabbitMqService
    {
        public static void AddMassTransitWithRabbitMqTransport(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
                ?? throw new InvalidOperationException("RabbitMQ configuration is missing.");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<BasketCheckoutConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.UserName);
                        h.Password(rabbitMqOptions.Password);
                    });
                    cfg.ReceiveEndpoint(rabbitMqOptions.ExchangeName, e =>
                    {
                        e.ConfigureConsumers(context);
                    });
                });
            });
        }
    }
}
