namespace Abstra.Domain.DTOs;

public class CreateStateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int CountryId { get; set; }
}
