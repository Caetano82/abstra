using Abstra.Repository.Data;
using Abstra.Domain.Contracts;
using Abstra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Abstra.Repository.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AbstraDbContext _context;

    public CountryRepository(AbstraDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        return await _context.Countries
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<Country> CreateAsync(Country country)
    {
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        return country;
    }

    public async Task<Country> UpdateAsync(Country country)
    {
        _context.Countries.Update(country);
        await _context.SaveChangesAsync();
        return country;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var country = await GetByIdAsync(id);
        if (country == null)
            return false;

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Countries.AnyAsync(c => c.Id == id);
    }
}
