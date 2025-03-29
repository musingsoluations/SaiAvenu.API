using ErrorOr;
using MediatR;
using SriSai.Application.Messaging.Command;

namespace SriSai.Application.Messaging.Handler
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            // Implementation of the command handler
            return Unit.Value;
        }
    }
}