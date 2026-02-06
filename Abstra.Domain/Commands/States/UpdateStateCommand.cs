using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.States;

public record UpdateStateCommand(int Id, UpdateStateRequest Request) : IRequest<StateDto?>;
