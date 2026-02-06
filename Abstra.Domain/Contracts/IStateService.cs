using Abstra.Domain.DTOs;

namespace Abstra.Domain.Contracts;

public interface IStateService
{
    Task<IEnumerable<StateDto>> GetAllAsync();
    Task<IEnumerable<StateDto>> GetByCountryIdAsync(int countryId);
    Task<StateDto?> GetByIdAsync(int id);
    Task<StateDto> CreateAsync(CreateStateRequest request);
    Task<StateDto?> UpdateAsync(int id, UpdateStateRequest request);
    Task<bool> DeleteAsync(int id);
}
