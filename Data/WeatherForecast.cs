#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syncfusion.GridPagingPerformance.Data
{
    public class WeatherForecast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime? Date { get; set; }

        public int? TemperatureC { get; set; }

        [NotMapped]
        public int TemperatureF => 32 + (int)(TemperatureC ?? 0 / 0.5556);

        public string? Summary { get; set; }
    }
}
