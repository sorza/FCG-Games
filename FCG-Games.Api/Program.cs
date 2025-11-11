using FCG_Games.Application.Shared;
using FCG_Games.Infrasctructure.Shared;
using FCG_Games.Infrasctructure.Shared.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FCG_Games.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registro de servi�os
            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<GamesDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Aplica migrations
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GamesDbContext>();
                db.Database.Migrate();
            }
            

            app.UseSwagger();
            app.UseSwaggerUI();            

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}