using com.github.akovac35.Logging;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.GridPagingPerformance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.GridPagingPerformance.Pages
{
    public partial class Index : ComponentBase, IDisposable
    {
        private bool disposedValue;

        [Inject]
        WeatherForecastContextFactory ContextFactory { get; set; }
        [Inject]
        ILogger<Index> Logger { get; set; }
        [Inject]
        CorrelationProviderAccessor CorrelationAccessor { get; set; }

        WeatherForecastContext Context { get; set; }
        IOrderedQueryable<WeatherForecast> Forecasts { get; set; }

        protected override void OnInitialized()
        {
            Logger.Here(l => l.Entering());

            base.OnInitialized();

            Context = ContextFactory.Create();
            Forecasts = Context.Forecasts.OrderBy(item => item.Id);

            Logger.Here(l => l.Exiting());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(Context != null)
                    {
                        Context.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Index()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
