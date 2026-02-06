using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.States;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class GetStateByIdQueryHandler : IRequestHandler<GetStateByIdQuery, Abstra.Domain.DTOs.StateDto?>
{
    private readonly IStateService _stateService;

    public GetStateByIdQueryHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<Abstra.Domain.DTOs.StateDto?> Handle(GetStateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _stateService.GetByIdAsync(request.Id);
    }
}
