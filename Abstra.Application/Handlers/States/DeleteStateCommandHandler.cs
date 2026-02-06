using Abstra.Domain.Commands.States;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class DeleteStateCommandHandler : IRequestHandler<DeleteStateCommand, bool>
{
    private readonly IStateService _stateService;

    public DeleteStateCommandHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<bool> Handle(DeleteStateCommand request, CancellationToken cancellationToken)
    {
        return await _stateService.DeleteAsync(request.Id);
    }
}
