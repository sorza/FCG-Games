using Azure.Messaging.ServiceBus;
using FCG_Games.Application.Shared.Interfaces;
using FCG_Games.Consumer.Consumers;
using FCG_Games.Infrastructure.Games.Repositories;
using FCG_Games.Infrastructure.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Consumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<GamesDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                    var connectionString = context.Configuration["ServiceBus:ConnectionString"];
                    services.AddSingleton(new ServiceBusClient(connectionString));                    

                    services.AddScoped<IGameRepository, GameRepository>();
                    services.AddHostedService<GamesTopicConsumer>();
                });

            await builder.RunConsoleAsync();
        }       
    }
}
