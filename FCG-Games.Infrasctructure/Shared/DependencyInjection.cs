using FCG_Games.Application.Shared.Interfaces.Repositories;
using FCG_Games.Infrasctructure.Games.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FCG_Games.Infrasctructure.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
