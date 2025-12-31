using FCG_Games.Api.Middlewares;
using FCG_Games.Application.Shared;
using FCG_Games.Infrastructure.Shared;
using FCG_Games.Infrastructure.Shared.Context;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FCG_Games.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://0.0.0.0:80");
            
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(80); 
            });

            var app = builder.Build();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GamesDbContext>();

                var retries = 5;
                while (retries > 0)
                {
                    try
                    {
                        db.Database.Migrate();
                        break;
                    }
                    catch
                    {
                        retries--;
                        Thread.Sleep(2000); 
                    }
                }
            }         

            app.UseSwagger();
            app.UseSwaggerUI();            

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.MapGet("/health", () =>
            {
                return Results.Ok(new
                {
                    status = "Healthy",
                    timestamp = DateTime.UtcNow
                });
            });
            
            app.Run();
        }
    }
}