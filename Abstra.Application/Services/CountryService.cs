using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Abstra.Domain.Entities;

namespace Abstra.Application.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _repository;

    public CountryService(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        var countries = await _repository.GetAllAsync();
        return countries.Select(MapToDto);
    }

    public async Task<CountryDto?> GetByIdAsync(int id)
    {
        var country = await _repository.GetByIdAsync(id);
        return country == null ? null : MapToDto(country);
    }

    public async Task<CountryDto> CreateAsync(CreateCountryRequest request)
    {
        var existingCountry = await _repository.GetByCodeAsync(request.Code);
        if (existingCountry != null)
        {
            throw new InvalidOperationException($"Country with code '{request.Code}' already exists.");
        }

        var country = new Country
        {
            Name = request.Name,
            Code = request.Code,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(country);
        return MapToDto(created);
    }

    public async Task<CountryDto?> UpdateAsync(int id, UpdateCountryRequest request)
    {
        var country = await _repository.GetByIdAsync(id);
        if (country == null)
            return null;

        var existingCountry = await _repository.GetByCodeAsync(request.Code);
        if (existingCountry != null && existingCountry.Id != id)
        {
            throw new InvalidOperationException($"Country with code '{request.Code}' already exists.");
        }

        country.Name = request.Name;
        country.Code = request.Code;

        var updated = await _repository.UpdateAsync(country);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static CountryDto MapToDto(Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            Code = country.Code,
            CreatedAt = country.CreatedAt
        };
    }
}
