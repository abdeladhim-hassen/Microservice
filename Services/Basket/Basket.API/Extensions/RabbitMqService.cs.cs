using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basket.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMassTransitWithRabbitMqTransport(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
                ?? throw new InvalidOperationException("RabbitMQ configuration is missing.");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.UserName);
                        h.Password(rabbitMqOptions.Password);
                    });
                });
            });
        }
    }
}
