using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.Cities;

public record GetCitiesByStateIdQuery(int StateId) : IRequest<IEnumerable<CityDto>>;
