using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;

namespace SriSai.Application.Collection.Command;

public record GetTotalAmountsCommand() : IRequest<ErrorOr<TotalAmountsDto>>; 