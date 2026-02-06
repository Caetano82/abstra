using Abstra.Domain.Commands.Cities;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Abstra.Domain.DTOs.CityDto>
{
    private readonly ICityService _cityService;

    public CreateCityCommandHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Abstra.Domain.DTOs.CityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        return await _cityService.CreateAsync(request.Request);
    }
}
