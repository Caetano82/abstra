using Abstra.Domain.Commands.Countries;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Countries;

public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, bool>
{
    private readonly ICountryService _countryService;

    public DeleteCountryCommandHandler(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        return await _countryService.DeleteAsync(request.Id);
    }
}
