using Abstra.Domain.Commands.Countries;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Countries;

public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Abstra.Domain.DTOs.CountryDto>
{
    private readonly ICountryService _countryService;

    public CreateCountryCommandHandler(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<Abstra.Domain.DTOs.CountryDto> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        return await _countryService.CreateAsync(request.Request);
    }
}
