using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.States;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery, IEnumerable<Abstra.Domain.DTOs.StateDto>>
{
    private readonly IStateService _stateService;

    public GetAllStatesQueryHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<IEnumerable<Abstra.Domain.DTOs.StateDto>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
    {
        return await _stateService.GetAllAsync();
    }
}
