using ErrorOr;
using MediatR;

namespace SriSai.Application.Messaging.Command
{
    public class SendMessageCommand : IRequest<ErrorOr<Unit>>
    {
        public string MobileNumber { get; set; }
        public string[] MessageBody { get; set; }
        public string TemplateName { get; set; }
    }
}