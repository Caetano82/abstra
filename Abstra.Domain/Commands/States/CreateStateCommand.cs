using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.States;

public record CreateStateCommand(CreateStateRequest Request) : IRequest<StateDto>;
