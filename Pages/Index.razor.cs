using com.github.akovac35.Logging;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.GridPagingPerformance.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.GridPagingPerformance.Pages
{
    public partial class Index : ComponentBase, IDisposable
    {
        bool disposedValue;
        
        [Inject]
        ILogger<Index> Logger { get; set; }
        
        [Inject]
        CorrelationProviderAccessor CorrelationAccessor { get; set; }

        int? TempCFilter { get; set; }
        DateTime LastRefresh = DateTime.Now;
        SfGrid<WeatherForecast> Grid { get; set; }

        protected override void OnInitialized()
        {
            Logger.Here(l => l.Entering());

            base.OnInitialized();

            Logger.Here(l => l.Exiting());
        }

        void OnTempCFilterChanged(int? value)
        {
            TempCFilter = value;
            Refresh();
        }

        void Refresh()
        {
            Grid.Refresh();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class CustomAdaptor : DataAdaptor
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Inject]
        ILogger<CustomAdaptor> Logger { get; set; }
        [Inject]
        WeatherForecastContextFactory WeatherForecastContextFactoryInstance { get; set; }

        [Parameter]
        public int? TempCFilter { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public override object Read(DataManagerRequest dm, string? key = null)
        {
            Logger.Here(l => l.Entering(dm.RequiresCounts, dm.Take, dm.Skip, TempCFilter!));

            using (var Context = WeatherForecastContextFactoryInstance.Create())
            {
                IQueryable<WeatherForecast> Requests = Context.Forecasts.AsQueryable();
                if (TempCFilter != null)
                    Requests = Requests.Where(item => item.TemperatureC == TempCFilter);
                Requests = Requests.OrderBy(item => item.Id).AsNoTracking();

                if (dm.Search != null && dm.Search.Count > 0)
                {
                    // Searching
                    Requests = DataOperations.PerformSearching(Requests, dm.Search);
                }
                if (dm.Sorted != null && dm.Sorted.Count > 0)
                {
                    // Sorting
                    Requests = DataOperations.PerformSorting(Requests, dm.Sorted);
                }
                if (dm.Where != null && dm.Where.Count > 0)
                {
                    // Filtering
                    Requests = DataOperations.PerformFiltering(Requests, dm.Where, dm.Where[0].Operator);
                }

                var pagedRequests = Requests;

                if (dm.Skip != 0)
                {
                    //Paging
                    pagedRequests = DataOperations.PerformSkip(Requests, dm.Skip);
                }
                if (dm.Take != 0)
                {
                    pagedRequests = DataOperations.PerformTake(Requests, dm.Take);
                }

                if (dm.RequiresCounts)
                {
                    int count = Requests.Count();

                    Logger.Here(l => l.Exiting(count));
                    return new DataResult() { Result = ProcessQuery(pagedRequests), Count = count };
                }
                else
                {
                    Logger.Here(l => l.Exiting());
                    return ProcessQuery(pagedRequests);
                }
            }
        }

        protected virtual List<WeatherForecast> ProcessQuery(IQueryable<WeatherForecast> query)
        {
            var tmp = query.ToList();
            return tmp;
        }
    }
}
