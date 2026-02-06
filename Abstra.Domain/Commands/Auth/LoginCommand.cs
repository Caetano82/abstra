using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.Auth;

public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;
