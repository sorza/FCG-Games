using Azure.Messaging.ServiceBus;
using FCG_Games.Application.Shared.Interfaces;
using FCG_Games.Infrastructure.Games.Events;
using FCG_Games.Infrastructure.Games.Repositories;
using FCG_Games.Infrastructure.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCG_Games.Infrastructure.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GamesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var queueName = configuration["ServiceBus:Queues:GamesEvents"];
            
            services.AddSingleton(new ServiceBusClient(connectionString));
           
            services.AddScoped<IEventPublisher>(sp =>
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                return new ServiceBusEventPublisher(client, queueName!);
            });

            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
