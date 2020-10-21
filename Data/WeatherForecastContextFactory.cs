using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Syncfusion.GridPagingPerformance.Data
{
    public class WeatherForecastContextFactory
    {
        public WeatherForecastContextFactory(DbContextOptions<WeatherForecastContext> options, ILoggerFactory loggerFactory)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public DbContextOptions<WeatherForecastContext> Options { get; }
        protected ILoggerFactory LoggerFactory { get; }

        public WeatherForecastContext Create()
        {
            return new WeatherForecastContext(Options, LoggerFactory);
        }
    }


}
