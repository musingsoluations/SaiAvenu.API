using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;

namespace SriSai.Application.Collection.Query;

public record GetUserPaymentsQuery(Guid UserId) : IRequest<ErrorOr<List<UserPaymentDto>>>;