using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.Countries;

public record GetAllCountriesQuery() : IRequest<IEnumerable<CountryDto>>;
