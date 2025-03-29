using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using SriSai.Application.Configuration;
using SriSai.Application.interfaces.ApiCalls;
using SriSai.Application.Messaging.Command;

namespace SriSai.Application.Messaging.Handler
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ErrorOr<Unit>>
    {
        private readonly IMessageSender _messageSender;
        private readonly WhatsAppConfiguration _whatsAppConfiguration;
        private readonly IDictionary<string, string> HeaderTemplates = new Dictionary<string, string>();

        public SendMessageCommandHandler(IMessageSender messageSender,
            IOptions<WhatsAppConfiguration> whatsAppConfiguration)
        {
            _messageSender = messageSender;
            _whatsAppConfiguration = whatsAppConfiguration.Value;
            HeaderTemplates.Add("SRISAI_1", "https://img.freepik.com/free-vector/paying-bills-concept-illustration_114360-22927.jpg?t=st=1743190756~exp=1743194356~hmac=ab6a2517650458bc806f14be22902fd38dffe047bf9c79853ff06047d424edb4&w=1380");
        }

        public async Task<ErrorOr<Unit>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var messageData = new WhatsAppMessageData
            {
                PhoneNumber = request.MobileNumber,
                Message = string.Join(",", request.MessageBody),
                TemplateName = request.TemplateName
            };

            if (HeaderTemplates.TryGetValue(request.TemplateName, out string? headerImagePath))
            {
                messageData.HeaderImagePath = headerImagePath;
            }
            else
            {
                messageData.HeaderImagePath = string.Empty;
            }

            var result = await _messageSender.SendMessage(messageData);
            return result.Match<ErrorOr<Unit>>(
                success => Unit.Value,
                errors => errors
            );
        }
    }
}