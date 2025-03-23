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
                x => x.RequestForDate.Year == request.Year,
                x => x.Payments);

            List<ChartDataItem> chartDataItems = new();
            List<string> months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                .Take(12)
                .ToList();

            foreach (string month in months)
            {
                ChartDataItem chartDataItem = new() { Name = month, Series = new List<SeriesItem>() };

                // Get demand for the month (based on RequestForDate)
                decimal monthDemand = result
                    .Where(x => x.RequestForDate.ToString("MMMM") == month)
                    .Sum(x => x.Amount);

                // Get collections for the month (based on Payment.PaidDate)
                decimal monthCollections = result
                    .SelectMany(x => x.Payments)
                    .Where(p => p.PaidDate.ToString("MMMM") == month &&
                                p.PaidDate.Year == request.Year)
                    .Sum(p => p.Amount);

                chartDataItem.Series.Add(new SeriesItem { Name = "Total Demand", Value = monthDemand });

                chartDataItem.Series.Add(new SeriesItem { Name = "Total Collection", Value = monthCollections });

                if (monthDemand > 0 || monthCollections > 0)
                {
                    chartDataItems.Add(chartDataItem);
                }
            }

            return chartDataItems;
        }
    }
}