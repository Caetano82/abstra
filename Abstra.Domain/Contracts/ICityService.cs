using Abstra.Domain.DTOs;

namespace Abstra.Domain.Contracts;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllAsync();
    Task<IEnumerable<CityDto>> GetByCountryIdAsync(int countryId);
    Task<IEnumerable<CityDto>> GetByStateIdAsync(int stateId);
    Task<CityDto?> GetByIdAsync(int id);
    Task<CityDto> CreateAsync(CreateCityRequest request);
    Task<CityDto?> UpdateAsync(int id, UpdateCityRequest request);
    Task<bool> DeleteAsync(int id);
}
