using Abstra.Domain.Entities;

namespace Abstra.Domain.Contracts;

public interface IStateRepository
{
    Task<IEnumerable<State>> GetAllAsync();
    Task<IEnumerable<State>> GetByCountryIdAsync(int countryId);
    Task<State?> GetByIdAsync(int id);
    Task<State?> GetByCodeAsync(string code, int countryId);
    Task<State> CreateAsync(State state);
    Task<State> UpdateAsync(State state);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
