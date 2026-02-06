using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.Countries;

public record UpdateCountryCommand(int Id, UpdateCountryRequest Request) : IRequest<CountryDto?>;
