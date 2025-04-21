namespace CatBoxAPI.Models.BoxRegistration;

public class BoxRegistrationEditDTO
{
    public required Guid Id { get; set; }
    public required string BoxType { get; set; }
    public required string BoxSize { get; set; }
    public string? SpecialFeatures { get; set; }
}
