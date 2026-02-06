using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.Countries;

public record GetCountryByIdQuery(int Id) : IRequest<CountryDto?>;
