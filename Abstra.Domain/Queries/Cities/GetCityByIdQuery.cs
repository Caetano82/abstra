using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.Cities;

public record GetCityByIdQuery(int Id) : IRequest<CityDto?>;
