namespace Abstra.Domain.DTOs;

public class UpdateStateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int CountryId { get; set; }
}
