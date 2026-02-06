using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.Cities;

public record UpdateCityCommand(int Id, UpdateCityRequest Request) : IRequest<CityDto?>;
