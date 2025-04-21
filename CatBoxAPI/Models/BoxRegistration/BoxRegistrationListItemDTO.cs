namespace CatBoxAPI.Models.BoxRegistration;

public class BoxRegistrationListItemDTO
{
    public required Guid Id { get; set; }
    public required Guid CatId { get; set; }
    public required string BoxType { get; set; }
    public required string BoxSize { get; set; }
    public string? SpecialFeatures { get; set; }
    public required bool? IsApproved { get; set; }
}
