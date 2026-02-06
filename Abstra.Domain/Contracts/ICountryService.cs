using Abstra.Domain.DTOs;

namespace Abstra.Domain.Contracts;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetAllAsync();
    Task<CountryDto?> GetByIdAsync(int id);
    Task<CountryDto> CreateAsync(CreateCountryRequest request);
    Task<CountryDto?> UpdateAsync(int id, UpdateCountryRequest request);
    Task<bool> DeleteAsync(int id);
}
