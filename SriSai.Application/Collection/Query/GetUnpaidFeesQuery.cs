using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;

namespace SriSai.Application.Collection.Query;

public record GetUnpaidFeesQuery() : IRequest<ErrorOr<List<UnpaidFeeResultDto>>>;
