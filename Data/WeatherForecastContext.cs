using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Syncfusion.GridPagingPerformance.Data
{
    public class WeatherForecastContext : DbContext
    {
        public WeatherForecastContext(DbContextOptions<WeatherForecastContext> options, ILoggerFactory loggerFactory) : this(options as DbContextOptions, loggerFactory)
        {
        }
        public WeatherForecastContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            LoggerFactory = loggerFactory;
        }

        protected ILoggerFactory LoggerFactory { get; set; }

        public DbSet<WeatherForecast> Forecasts => Set<WeatherForecast>();
    }
}
