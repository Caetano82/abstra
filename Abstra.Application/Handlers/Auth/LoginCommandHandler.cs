using Abstra.Domain.Commands.Auth;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Abstra.Domain.DTOs.LoginResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Abstra.Domain.DTOs.LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Request);
    }
}
