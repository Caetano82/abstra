using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Commands.Cities;

public record CreateCityCommand(CreateCityRequest Request) : IRequest<CityDto>;
