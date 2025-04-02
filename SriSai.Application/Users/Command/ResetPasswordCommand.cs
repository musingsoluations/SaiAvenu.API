using ErrorOr;
using MediatR;

namespace SriSai.Application.Users.Command
{
    public record ResetPasswordCommand(
        string Mobile) : IRequest<ErrorOr<string>>;
}