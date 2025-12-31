using Azure.Messaging.ServiceBus;
using FCG.Shared.Contracts.Interfaces;
using FCG_Games.Application.Shared.Interfaces;
using FCG_Games.Infrastructure.Games.Events;
using FCG_Games.Infrastructure.Games.Repositories;
using FCG_Games.Infrastructure.Mongo;
using FCG_Games.Infrastructure.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FCG_Games.Infrastructure.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GamesDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var topic = configuration["ServiceBus:Topics:Games"];
            
            services.AddSingleton(new ServiceBusClient(connectionString));
           
            services.AddScoped<IEventPublisher>(sp =>
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                return new ServiceBusEventPublisher(client, topic!);
            });

            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoString = configuration["MongoSettings:ConnectionString"];
                return new MongoClient(mongoString);
            });


            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IEventStore, MongoEventStore>();

            return services;
        }
    }
}
