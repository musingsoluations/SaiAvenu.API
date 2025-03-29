using System.Net.Http;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SriSai.Application.Configuration;
using SriSai.Application.interfaces.ApiCalls;

namespace SriSai.infrastructure.ApiCall
{
    public class MessageSender : IMessageSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MessageSender> _logger;
        private readonly WhatsAppConfiguration _whatsAppConfiguration;

        public MessageSender(IHttpClientFactory httpClientFactory, IOptions<WhatsAppConfiguration> options, ILogger<MessageSender> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _whatsAppConfiguration = options.Value;
        }

        public async Task<ErrorOr<bool>> SendMessage(WhatsAppMessageData messageData)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                
                // Build the URL with query parameters
                var queryParams = new List<string>
                {
                    $"PHID={_whatsAppConfiguration.SenderId}",
                    $"MobileNo={messageData.PhoneNumber}",
                    $"MessageBody={messageData.Message}",
                    $"TemplName={messageData.TemplateName}",
                    $"token={_whatsAppConfiguration.ApiKey}"
                };

                // Add header parameters if provided
                if (!string.IsNullOrEmpty(messageData.HeaderImagePath))
                {
                    queryParams.Add("headerType=Image");
                    queryParams.Add($"HeaderValue={messageData.HeaderImagePath}");
                }

                var url = $"{_whatsAppConfiguration.BaseUrl}?{string.Join("&", queryParams)}";
                
                _logger.LogInformation("Sending WhatsApp message to URL: {Url}", url);
                
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                _logger.LogInformation("WhatsApp message sent successfully");
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to send WhatsApp message");
                return Error.Failure("Failed to send WhatsApp message", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending WhatsApp message");
                return Error.Failure("Unexpected error", ex.Message);
            }
        }
    }
}