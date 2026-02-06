using Abstra.Domain.Commands.States;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.States;

public class CreateStateCommandHandler : IRequestHandler<CreateStateCommand, Abstra.Domain.DTOs.StateDto>
{
    private readonly IStateService _stateService;

    public CreateStateCommandHandler(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<Abstra.Domain.DTOs.StateDto> Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        return await _stateService.CreateAsync(request.Request);
    }
}
