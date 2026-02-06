using Abstra.Domain.Commands.Countries;
using Abstra.Domain.Contracts;
using Abstra.Domain.Queries.Countries;
using MediatR;

namespace Abstra.Application.Handlers.Countries;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<Abstra.Domain.DTOs.CountryDto>>
{
    private readonly ICountryService _countryService;

    public GetAllCountriesQueryHandler(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<IEnumerable<Abstra.Domain.DTOs.CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        return await _countryService.GetAllAsync();
    }
}
