using Abstra.Domain.Commands.Cities;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Abstra.Domain.DTOs.CityDto?>
{
    private readonly ICityService _cityService;

    public UpdateCityCommandHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Abstra.Domain.DTOs.CityDto?> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        return await _cityService.UpdateAsync(request.Id, request.Request);
    }
}
