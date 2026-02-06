using Abstra.Domain.Entities;

namespace Abstra.Domain.Contracts;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync();
    Task<IEnumerable<City>> GetByCountryIdAsync(int countryId);
    Task<IEnumerable<City>> GetByStateIdAsync(int stateId);
    Task<City?> GetByIdAsync(int id);
    Task<City> CreateAsync(City city);
    Task<City> UpdateAsync(City city);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
