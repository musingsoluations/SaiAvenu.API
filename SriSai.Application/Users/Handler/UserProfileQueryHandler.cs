using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler
{
    public class UserProfileQueryHandler : IRequestHandler<UserProfileQuery, ErrorOr<UserProfile>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UserProfile>> Handle(UserProfileQuery request, CancellationToken cancellationToken)
        {
            UserEntity? user = await _unitOfWork.Repository<UserEntity>().GetByIdAsync(request.userId);
            if (user is null)
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            return new UserProfile(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Mobile);
        }
    }
}