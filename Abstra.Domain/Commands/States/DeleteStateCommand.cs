using MediatR;

namespace Abstra.Domain.Commands.States;

public record DeleteStateCommand(int Id) : IRequest<bool>;
