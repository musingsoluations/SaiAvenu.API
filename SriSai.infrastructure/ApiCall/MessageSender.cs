using System.Net.Http;
using ErrorOr;
using Microsoft.Extensions.Logging;
using SriSai.Application.interfaces.ApiCalls;

namespace SriSai.infrastructure.ApiCall
{
    public class MessageSender : IMessageSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MessageSender> _logger;

        public MessageSender(IHttpClientFactory httpClientFactory, ILogger<MessageSender> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> SendMessage(string url)
        {
            try
            {
                _logger.LogInformation("Sending HTTP GET request to {Url}", url);
                
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);
                
                response.EnsureSuccessStatusCode();
                
                _logger.LogInformation("Successfully sent HTTP GET request to {Url}", url);
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error sending HTTP GET request to {Url}", url);
                return Error.Failure("MessageSender.SendFailed", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending HTTP GET request to {Url}", url);
                return Error.Unexpected("MessageSender.UnexpectedError", ex.Message);
            }
        }
    }
}