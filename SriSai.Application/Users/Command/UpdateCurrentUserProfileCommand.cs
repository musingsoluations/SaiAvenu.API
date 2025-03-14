using ErrorOr;
using MediatR;
using SriSai.Application.Users.Query;

namespace SriSai.Application.Users.Command;

public class UpdateCurrentUserProfileCommand : IRequest<ErrorOr<UserProfile>>
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Mobile { get; set; }
    public string? Password { get; set; }
}