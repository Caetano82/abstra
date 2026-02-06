using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Abstra.Domain.Entities;

namespace Abstra.Application.Services;

public class StateService : IStateService
{
    private readonly IStateRepository _stateRepository;
    private readonly ICountryRepository _countryRepository;

    public StateService(IStateRepository stateRepository, ICountryRepository countryRepository)
    {
        _stateRepository = stateRepository;
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<StateDto>> GetAllAsync()
    {
        var states = await _stateRepository.GetAllAsync();
        return states.Select(MapToDto);
    }

    public async Task<IEnumerable<StateDto>> GetByCountryIdAsync(int countryId)
    {
        var states = await _stateRepository.GetByCountryIdAsync(countryId);
        return states.Select(MapToDto);
    }

    public async Task<StateDto?> GetByIdAsync(int id)
    {
        var state = await _stateRepository.GetByIdAsync(id);
        return state == null ? null : MapToDto(state);
    }

    public async Task<StateDto> CreateAsync(CreateStateRequest request)
    {
        var countryExists = await _countryRepository.ExistsAsync(request.CountryId);
        if (!countryExists)
        {
            throw new InvalidOperationException($"Country with ID {request.CountryId} does not exist.");
        }

        var country = await _countryRepository.GetByIdAsync(request.CountryId);
        if (country == null)
        {
            throw new InvalidOperationException($"Country with ID {request.CountryId} does not exist.");
        }

        var existingState = await _stateRepository.GetByCodeAsync(request.Code, request.CountryId);
        if (existingState != null)
        {
            throw new InvalidOperationException($"State with code '{request.Code}' already exists in this country.");
        }

        var state = new State
        {
            Name = request.Name,
            Code = request.Code,
            CountryId = request.CountryId,
            CreatedAt = DateTime.UtcNow,
            Country = country
        };

        var created = await _stateRepository.CreateAsync(state);
        
        created.Country = country;
        return MapToDto(created);
    }

    public async Task<StateDto?> UpdateAsync(int id, UpdateStateRequest request)
    {
        var state = await _stateRepository.GetByIdAsync(id);
        if (state == null)
            return null;

        var countryExists = await _countryRepository.ExistsAsync(request.CountryId);
        if (!countryExists)
        {
            throw new InvalidOperationException($"Country with ID {request.CountryId} does not exist.");
        }

        var country = await _countryRepository.GetByIdAsync(request.CountryId);
        if (country == null)
        {
            throw new InvalidOperationException($"Country with ID {request.CountryId} does not exist.");
        }

        var existingState = await _stateRepository.GetByCodeAsync(request.Code, request.CountryId);
        if (existingState != null && existingState.Id != id)
        {
            throw new InvalidOperationException($"State with code '{request.Code}' already exists in this country.");
        }

        state.Name = request.Name;
        state.Code = request.Code;
        state.CountryId = request.CountryId;
        state.Country = country;

        var updated = await _stateRepository.UpdateAsync(state);
        
        updated.Country = country;
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _stateRepository.DeleteAsync(id);
    }

    private static StateDto MapToDto(State state)
    {
        return new StateDto
        {
            Id = state.Id,
            Name = state.Name,
            Code = state.Code,
            CountryId = state.CountryId,
            CountryName = state.Country?.Name,
            CreatedAt = state.CreatedAt
        };
    }
}
