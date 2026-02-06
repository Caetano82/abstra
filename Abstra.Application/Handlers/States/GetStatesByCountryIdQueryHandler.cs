using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.States;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class GetStatesByCountryIdQueryHandler : IRequestHandler<GetStatesByCountryIdQuery, IEnumerable<Abstra.Domain.DTOs.StateDto>>
{
    private readonly IStateService _stateService;

    public GetStatesByCountryIdQueryHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<IEnumerable<Abstra.Domain.DTOs.StateDto>> Handle(GetStatesByCountryIdQuery request, CancellationToken cancellationToken)
    {
        return await _stateService.GetByCountryIdAsync(request.CountryId);
    }
}
