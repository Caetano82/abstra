using Abstra.Domain.Commands.Cities;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Cities;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, bool>
{
    private readonly ICityService _cityService;

    public DeleteCityCommandHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        return await _cityService.DeleteAsync(request.Id);
    }
}
