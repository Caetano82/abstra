using Abstra.Repository.Data;
using Abstra.Domain.Contracts;
using Abstra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Abstra.Repository.Repositories;

public class StateRepository : IStateRepository
{
    private readonly AbstraDbContext _context;

    public StateRepository(AbstraDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<State>> GetAllAsync()
    {
        return await _context.States
            .Include(s => s.Country)
            .ToListAsync();
    }

    public async Task<IEnumerable<State>> GetByCountryIdAsync(int countryId)
    {
        return await _context.States
            .Include(s => s.Country)
            .Where(s => s.CountryId == countryId)
            .ToListAsync();
    }

    public async Task<State?> GetByIdAsync(int id)
    {
        return await _context.States
            .Include(s => s.Country)
            .Include(s => s.Cities)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<State?> GetByCodeAsync(string code, int countryId)
    {
        return await _context.States
            .FirstOrDefaultAsync(s => s.Code == code && s.CountryId == countryId);
    }

    public async Task<State> CreateAsync(State state)
    {
        _context.States.Add(state);
        await _context.SaveChangesAsync();
        return state;
    }

    public async Task<State> UpdateAsync(State state)
    {
        _context.States.Update(state);
        await _context.SaveChangesAsync();
        return state;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var state = await GetByIdAsync(id);
        if (state == null)
            return false;

        _context.States.Remove(state);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.States.AnyAsync(s => s.Id == id);
    }
}
