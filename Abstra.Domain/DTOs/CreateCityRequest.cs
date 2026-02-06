namespace Abstra.Domain.DTOs;

public class CreateCityRequest
{
    public string Name { get; set; } = string.Empty;
    public int StateId { get; set; }
    public int? Population { get; set; }
}
