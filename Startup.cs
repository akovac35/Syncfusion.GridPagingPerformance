using com.github.akovac35.Logging.AspNetCore.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Syncfusion.Blazor;
using Syncfusion.GridPagingPerformance.Data;

namespace Syncfusion.GridPagingPerformance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.TryAddSingleton<DbContextOptions<WeatherForecastContext>>(fact =>
            {
                var conn = fact.GetRequiredService<SqliteConnection>();
                var loggingFact = fact.GetRequiredService<ILoggerFactory>();
                var tmp = new DbContextOptionsBuilder<WeatherForecastContext>().UseSqlite(conn).UseLoggerFactory(loggingFact);
                tmp.EnableSensitiveDataLogging();
                return tmp.Options;
            });
            services.TryAddSingleton(fact =>
            {
                var conn = new SqliteConnection("Filename=:memory:");
                conn.Open();
                return conn;
            });
            services.TryAddSingleton<WeatherForecastContextFactory>();
            services.AddLoggingCorrelation();
            services.AddSyncfusionBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();
            app.UseLoggingCorrelation();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
