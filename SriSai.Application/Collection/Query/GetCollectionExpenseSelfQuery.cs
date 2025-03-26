using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;

namespace SriSai.Application.Collection.Query
{
    public class GetCollectionExpenseSelfQuery : IRequest<ErrorOr<List<ChartDataItem>>>
    {
        public int Year { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}