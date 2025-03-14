using ErrorOr;
using MediatR;

namespace SriSai.Application.Users.Query;

public record UserProfileQuery(Guid userId) : IRequest<ErrorOr<UserProfile>>;

public record UserProfile(
    string FirstName,
    string LastName,
    string Email,
    string Mobile);