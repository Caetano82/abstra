namespace Abstra.Domain.DTOs;

public class StateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public DateTime CreatedAt { get; set; }
}
