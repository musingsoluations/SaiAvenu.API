using ErrorOr;
using MediatR;

namespace SriSai.Application.Users.Query;

public record ValidateUserQuery(string Mobile, string Password) : IRequest<ErrorOr<UserProfileResponse>>;

public record UserProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Mobile,
    List<string> Roles);