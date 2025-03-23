using SriSai.Application.Collection.Query;

namespace SriSai.Application.Collection.Dtos
{
    public class ChartDataItem
    {
        public string Name { get; set; } // Month name (e.g., "January")
        public List<SeriesItem> Series { get; set; }
    }
}