using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.Cities;

public record GetAllCitiesQuery() : IRequest<IEnumerable<CityDto>>;
