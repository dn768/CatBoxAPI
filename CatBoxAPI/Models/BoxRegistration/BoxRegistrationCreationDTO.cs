namespace CatBoxAPI.Models.BoxRegistration;

public class BoxRegistrationCreationDTO
{
    public required Guid CatId { get; set; }
    public required string BoxType { get; set; }
    public required string BoxSize { get; set; }
    public string? SpecialFeatures { get; set; }
}
