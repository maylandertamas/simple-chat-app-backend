using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApp.Data;

namespace SimpleChatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(logContext => logContext.File("Logs/log.txt"))
                .CreateLogger();
            try
            {
                Log.Information("Starting application...");
#if DEBUG                
                CreateHostBuilder(args).Build().Run();
#else           
                // If production build executes update the database with the migrations before run
                CreateHostBuilder(args).Build().MigrateDatabase<DataContext>().Run();
#endif
            }
            catch (Exception e)
            {
                Log.Fatal("Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
#if DEBUG
                    config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);      
#elif DEPLOY
                    config.AddJsonFile("appsettings.Prod.json");
#else
                    config.AddJsonFile("appsettings.Development.json");
#endif
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://0.0.0.0:4000");
                })
                .UseSerilog();
    }
    /// <summary>
    /// Migrate database before run
    /// </summary>
    public static class MigrationHelper
    {
        /// <summary>
        /// Migrate database before application starts to run
        /// </summary>
        /// <param name="host"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"An error occurred while migrating the database.  {ex.Message}");
                }
            }
            return host;
        }
    }
}
