using MediatR;

namespace Abstra.Domain.Commands.Cities;

public record DeleteCityCommand(int Id) : IRequest<bool>;
