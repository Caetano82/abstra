namespace Abstra.Domain.DTOs;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StateId { get; set; }
    public string? StateName { get; set; }
    public int? CountryId { get; set; }
    public string? CountryName { get; set; }
    public int? Population { get; set; }
    public DateTime CreatedAt { get; set; }
}
