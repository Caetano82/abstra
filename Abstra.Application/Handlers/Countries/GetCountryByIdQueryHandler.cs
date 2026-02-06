using Abstra.Domain.Queries.Countries;
using Abstra.Domain.Contracts;
using MediatR;

namespace Abstra.Application.Handlers.Countries;

public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Abstra.Domain.DTOs.CountryDto?>
{
    private readonly ICountryService _countryService;

    public GetCountryByIdQueryHandler(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<Abstra.Domain.DTOs.CountryDto?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _countryService.GetByIdAsync(request.Id);
    }
}
