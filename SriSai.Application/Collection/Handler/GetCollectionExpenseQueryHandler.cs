using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;
using System.Globalization;

namespace SriSai.Application.Collection.Handler
{
    public class
        GetCollectionExpenseQueryHandler : IRequestHandler<GetCollectionExpenseQuery, ErrorOr<List<ChartDataItem>>>
    {
        private readonly IRepository<FeeCollectionEntity> _feeCollectionRepo;

        public GetCollectionExpenseQueryHandler(IRepository<FeeCollectionEntity> feeCollectionRepo)
        {
            _feeCollectionRepo = feeCollectionRepo;
        }

        public async Task<ErrorOr<List<ChartDataItem>>> Handle(GetCollectionExpenseQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<FeeCollectionEntity> result = await _feeCollectionRepo.FindAllWithIncludeAsync(
                x => x.RequestForDate.Year == request.Year, x => x.Payments);

            List<ChartDataItem> chartDataItems = new();
            List<string> months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            foreach (string month in months)
            {
                ChartDataItem chartDataItem = new() { Name = month, Series = new List<SeriesItem>() };
                IEnumerable<FeeCollectionEntity> monthData =
                    result.Where(x => x.RequestForDate.ToString("MMMM") == month);
                SeriesItem seriesItemDemand = new() { Name = "Total Demand", Value = monthData.Sum(x => x.Amount) };
                SeriesItem seriesItemCollected = new()
                {
                    Name = "Total Collection", Value = monthData.Sum(x => x.Payments.Sum(p => p.Amount))
                };

                chartDataItem.Series.Add(seriesItemDemand);
                chartDataItem.Series.Add(seriesItemCollected);
                if (seriesItemDemand.Value > 0 || seriesItemCollected.Value > 0)
                {
                    chartDataItems.Add(chartDataItem);
                }
            }

            return chartDataItems;
        }
    }
}