using Abstra.Repository.Data;
using Abstra.Domain.Contracts;
using Abstra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Abstra.Repository.Repositories;

public class CityRepository : ICityRepository
{
    private readonly AbstraDbContext _context;

    public CityRepository(AbstraDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context.Cities
            .Include(c => c.State)
                .ThenInclude(s => s.Country)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetByCountryIdAsync(int countryId)
    {
        return await _context.Cities
            .Include(c => c.State)
                .ThenInclude(s => s.Country)
            .Where(c => c.State.CountryId == countryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetByStateIdAsync(int stateId)
    {
        return await _context.Cities
            .Include(c => c.State)
                .ThenInclude(s => s.Country)
            .Where(c => c.StateId == stateId)
            .ToListAsync();
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _context.Cities
            .Include(c => c.State)
                .ThenInclude(s => s.Country)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<City> CreateAsync(City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();
        return city;
    }

    public async Task<City> UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
        return city;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var city = await GetByIdAsync(id);
        if (city == null)
            return false;

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Cities.AnyAsync(c => c.Id == id);
    }
}
