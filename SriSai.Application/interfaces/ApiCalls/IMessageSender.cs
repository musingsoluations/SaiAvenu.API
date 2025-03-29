using ErrorOr;

namespace SriSai.Application.interfaces.ApiCalls
{
    /// <summary>
    /// Interface for sending messages to external services via HTTP
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends a GET request to the specified URL
        /// </summary>
        /// <param name="url">The URL to send the request to</param>
        /// <returns>ErrorOr of success or failure</returns>
        Task<ErrorOr<bool>> SendMessage(string url);
    }
}