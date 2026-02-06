using Abstra.Domain.Entities;

namespace Abstra.Domain.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task<Country?> GetByCodeAsync(string code);
    Task<Country> CreateAsync(Country country);
    Task<Country> UpdateAsync(Country country);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
