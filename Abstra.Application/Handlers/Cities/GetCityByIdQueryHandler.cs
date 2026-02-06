using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.Cities;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Abstra.Domain.DTOs.CityDto?>
{
    private readonly ICityService _cityService;

    public GetCityByIdQueryHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Abstra.Domain.DTOs.CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        return await _cityService.GetByIdAsync(request.Id);
    }
}
