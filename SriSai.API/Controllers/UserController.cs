using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Users;
using SriSai.API.Services.Auth;
using SriSai.Application.Users.Command;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;

namespace SriSai.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserDto> _validator;

    public UserController(
        IMediator mediator,
        IValidator<CreateUserDto> validator,
        IJwtTokenService jwtTokenService)
    {
        _mediator = mediator;
        _validator = validator;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    [Authorize("AdminOnly")]
    public async Task<IActionResult> AddUser(CreateUserDto createUserDto)
    {
        var validationResult = await _validator.ValidateAsync(createUserDto);
        if (validationResult.IsValid == false)
            return new ObjectResult(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred",
                Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage).ToArray() }
            });

        var userGuid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        // Create UserRole objects from string roles
        var roles = createUserDto.Roles.Select(roleString =>
            new UserRole { UserRoleName = roleString }).ToList();

        var command = new CreateUserCommand
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Password = createUserDto.Password,
            Mobile = createUserDto.Mobile,
            Roles = roles,
            CreatedById = Guid.Parse(userGuid ?? string.Empty)
        };

        var result = await _mediator.Send(command);

        return result.Match(
            userId => Ok(userId),
            errors => new ObjectResult(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = result.Errors.FirstOrDefault().Description,
                Extensions =
                {
                    ["errors"] = result.Errors.FirstOrDefault().Code
                }
            }));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var query = new ValidateUserQuery(loginUserDto.MobileNumber, loginUserDto.Password);
        var result = await _mediator.Send(query);
        return result.Match(
            user => Ok(new UserInformationDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.Mobile,
                JwtToken = _jwtTokenService.GenerateToken(user)
            }),
            errors => Problem(string.Join(", ", errors.Select(e => e.Description)))
        );
    }
}