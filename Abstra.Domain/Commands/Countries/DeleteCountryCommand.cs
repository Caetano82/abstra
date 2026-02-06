using MediatR;

namespace Abstra.Domain.Commands.Countries;

public record DeleteCountryCommand(int Id) : IRequest<bool>;
