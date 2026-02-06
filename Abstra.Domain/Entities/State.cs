namespace Abstra.Domain.Entities;

public class State
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Country Country { get; set; } = null!;
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
