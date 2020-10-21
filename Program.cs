using com.github.akovac35.Logging;
using com.github.akovac35.Logging.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.GridPagingPerformance.Data;
using System;
using Serilog;

using static com.github.akovac35.Logging.LoggerHelper<Syncfusion.GridPagingPerformance.Program>;

namespace Syncfusion.GridPagingPerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SerilogHelper.CreateLogger(configure => configure.AddJsonFile("serilog.json", optional: false, reloadOnChange: true));
            LoggerFactoryProvider.LoggerFactory = SerilogHelper.CreateLoggerFactory();

            var host = CreateHostBuilder(args).Build();

            using (var context = host.Services.GetRequiredService<WeatherForecastContextFactory>().Create())
            {
                context.Database.EnsureCreated();

                Logger.Here(l => l.LogWarning("Preparing mocked data, please wait ..."));

                var ws = host.Services.GetRequiredService<WeatherForecastService>();
                context.Forecasts.AddRange(ws.GetForecastAsync(DateTime.Now).Result);
                context.SaveChanges();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging =>
                    {
                        // Needed to remove duplicate log entries
                        logging.ClearProviders();
                    }).UseSerilog();
                });
    }
}
