using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.Cities;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<Abstra.Domain.DTOs.CityDto>>
{
    private readonly ICityService _cityService;

    public GetAllCitiesQueryHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<IEnumerable<Abstra.Domain.DTOs.CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _cityService.GetAllAsync();
    }
}
