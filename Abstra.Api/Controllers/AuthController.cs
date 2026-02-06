using Abstra.Domain.Commands.Auth;
using Abstra.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abstra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest? request)
    {
        try
        {
            _logger.LogInformation("Login attempt received. Request is null: {IsNull}", request == null);
            
            if (request == null)
            {
                _logger.LogWarning("Login request is null");
                return BadRequest(new { error = "Request body is required" });
            }

            _logger.LogInformation("Login attempt for username: '{Username}', password length: {PasswordLength}", 
                request.Username ?? "null", 
                request.Password?.Length ?? 0);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Login attempt with empty username or password");
                return Unauthorized(new { error = "Username and password are required" });
            }

            var command = new LoginCommand(request);
            var response = await _mediator.Send(command);
            if (response == null)
            {
                _logger.LogWarning("Login failed - LoginCommandHandler returned null for username: {Username}", request.Username);
                return Unauthorized(new { error = "Invalid username or password" });
            }

            _logger.LogInformation("Login successful for username: {Username}", request.Username);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { error = "An error occurred during login" });
        }
    }
}
