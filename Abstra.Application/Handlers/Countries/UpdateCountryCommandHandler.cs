using Abstra.Domain.Commands.Countries;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Countries;

public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Abstra.Domain.DTOs.CountryDto?>
{
    private readonly ICountryService _countryService;

    public UpdateCountryCommandHandler(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<Abstra.Domain.DTOs.CountryDto?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        return await _countryService.UpdateAsync(request.Id, request.Request);
    }
}
