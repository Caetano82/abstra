namespace Abstra.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StateId { get; set; }
    public int? Population { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual State State { get; set; } = null!;
}
