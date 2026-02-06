using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.Countries;

public record CreateCountryCommand(CreateCountryRequest Request) : IRequest<CountryDto>;
