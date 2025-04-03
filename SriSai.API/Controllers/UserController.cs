using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Users;
using SriSai.API.Services.Auth;
using SriSai.Application.Users.Command;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using System.Security.Claims;

namespace SriSai.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<UserProfileDto> _userProfileValidator;
        private readonly IValidator<CreateUserDto> _validator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;

        public UserController(
            IMediator mediator,
            IValidator<CreateUserDto> validator,
            IJwtTokenService jwtTokenService,
            IValidator<UserProfileDto> userProfileValidator,
            IValidator<ResetPasswordDto> resetPasswordValidator,
            ILogger<UserController> logger)
        {
            _mediator = mediator;
            _validator = validator;
            _jwtTokenService = jwtTokenService;
            _userProfileValidator = userProfileValidator;
            _resetPasswordValidator = resetPasswordValidator;
            _logger = logger;
        }

        [HttpPost("register")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> AddUser(CreateUserDto createUserDto)
        {
            _logger.LogInformation("Starting user registration process for: {FirstName} {LastName}",
                createUserDto.FirstName, createUserDto.LastName);

            ValidationResult? validationResult = await _validator.ValidateAsync(createUserDto);
            if (validationResult.IsValid == false)
            {
                return new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred",
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() }
                });
            }

            string? userGuid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            // Create UserRole objects from string roles
            List<UserRoleEntity> roles = createUserDto.Roles.Select(roleString =>
                new UserRoleEntity { UserRoleName = roleString }).ToList();

            CreateUserCommand command = new()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Password = createUserDto.Password,
                Mobile = createUserDto.Mobile,
                Roles = roles,
                CreatedById = Guid.Parse(userGuid ?? string.Empty)
            };

            ErrorOr<Guid> result = await _mediator.Send(command);

            return result.Match(
                userId => Ok(userId),
                errors => new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            _logger.LogInformation("Getting user profile");
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            UserProfileQuery query = new(Guid.Parse(userId ?? string.Empty));
            ErrorOr<UserProfile> result = await _mediator.Send(query);
            return result.Match(
                userProfile => Ok(userProfile),
                errors =>
                    Problem(string.Join(", ", errors.Select(e => e.Description))));
        }

        [HttpPatch("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile(UserProfileDto userProfile)
        {
            _logger.LogInformation("Updating user profile for user: {UserId}", User.Identity?.Name);

            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            ValidationResult? validationResult = await _userProfileValidator.ValidateAsync(userProfile);
            if (validationResult.IsValid == false)
            {
                return new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred",
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() }
                });
            }

            Guid userGuid = Guid.Parse(userId);
            UpdateCurrentUserProfileCommand command = new()
            {
                Id = userGuid,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                Password = userProfile.Password,
                Mobile = userProfile.Mobile
            };
            ErrorOr<UserProfile> result = await _mediator.Send(command);
            return result.Match(
                user => Ok(result.Value),
                errors => new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }

        // force deploy to test
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            ValidateUserQuery query = new(loginUserDto.MobileNumber, loginUserDto.Password);
            _logger.LogInformation("Starting login process for user: {MobileNumber}", loginUserDto.MobileNumber);
            ErrorOr<UserProfileResponse> result = await _mediator.Send(query);
            return result.Match(
                user => Ok(new UserInformationDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    JwtToken = _jwtTokenService.GenerateToken(user)
                }),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }

        [HttpPost("reset-password")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            _logger.LogInformation("Starting password reset process for mobile: {MobileNumber}", resetPasswordDto.MobileNumber);

            ValidationResult? validationResult = await _resetPasswordValidator.ValidateAsync(resetPasswordDto);
            if (validationResult.IsValid == false)
            {
                return new ObjectResult(new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred",
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() }
                });
            }

            // Create and send the reset password command
            ResetPasswordCommand command = new(resetPasswordDto.MobileNumber);
            ErrorOr<string> result = await _mediator.Send(command);

            return result.Match(
                newPassword => Ok(new { Message = "Password has been reset successfully", NewPassword = newPassword }),
                errors => new ObjectResult(new ProblemDetails
                {
                    Status = 404,
                    Title = "Reset Password Error",
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }
    }
}