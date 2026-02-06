using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Abstra.Domain.Entities;

namespace Abstra.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IStateRepository _stateRepository;

    public CityService(ICityRepository cityRepository, IStateRepository stateRepository)
    {
        _cityRepository = cityRepository;
        _stateRepository = stateRepository;
    }

    public async Task<IEnumerable<CityDto>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return cities.Select(MapToDto);
    }

    public async Task<IEnumerable<CityDto>> GetByCountryIdAsync(int countryId)
    {
        var cities = await _cityRepository.GetByCountryIdAsync(countryId);
        return cities.Select(MapToDto);
    }

    public async Task<IEnumerable<CityDto>> GetByStateIdAsync(int stateId)
    {
        var cities = await _cityRepository.GetByStateIdAsync(stateId);
        return cities.Select(MapToDto);
    }

    public async Task<CityDto?> GetByIdAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        return city == null ? null : MapToDto(city);
    }

    public async Task<CityDto> CreateAsync(CreateCityRequest request)
    {
        var state = await _stateRepository.GetByIdAsync(request.StateId);
        if (state == null)
        {
            throw new InvalidOperationException($"State with ID {request.StateId} does not exist.");
        }

        var city = new City
        {
            Name = request.Name,
            StateId = request.StateId,
            Population = request.Population,
            CreatedAt = DateTime.UtcNow,
            State = state
        };

        var created = await _cityRepository.CreateAsync(city);
        
        created.State = state;
        return MapToDto(created);
    }

    public async Task<CityDto?> UpdateAsync(int id, UpdateCityRequest request)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            return null;

        var state = await _stateRepository.GetByIdAsync(request.StateId);
        if (state == null)
        {
            throw new InvalidOperationException($"State with ID {request.StateId} does not exist.");
        }

        city.Name = request.Name;
        city.StateId = request.StateId;
        city.Population = request.Population;
        city.State = state;

        var updated = await _cityRepository.UpdateAsync(city);
        
        updated.State = state;
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _cityRepository.DeleteAsync(id);
    }

    private static CityDto MapToDto(City city)
    {
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            StateId = city.StateId,
            StateName = city.State?.Name,
            CountryId = city.State?.CountryId,
            CountryName = city.State?.Country?.Name,
            Population = city.Population,
            CreatedAt = city.CreatedAt
        };
    }
}
