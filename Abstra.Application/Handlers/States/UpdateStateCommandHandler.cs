using Abstra.Domain.Commands.States;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class UpdateStateCommandHandler : IRequestHandler<UpdateStateCommand, Abstra.Domain.DTOs.StateDto?>
{
    private readonly IStateService _stateService;

    public UpdateStateCommandHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<Abstra.Domain.DTOs.StateDto?> Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        return await _stateService.UpdateAsync(request.Id, request.Request);
    }
}
