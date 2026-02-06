using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.Cities;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class GetCitiesByStateIdQueryHandler : IRequestHandler<GetCitiesByStateIdQuery, IEnumerable<Abstra.Domain.DTOs.CityDto>>
{
    private readonly ICityService _cityService;

    public GetCitiesByStateIdQueryHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<IEnumerable<Abstra.Domain.DTOs.CityDto>> Handle(GetCitiesByStateIdQuery request, CancellationToken cancellationToken)
    {
        return await _cityService.GetByStateIdAsync(request.StateId);
    }
}
