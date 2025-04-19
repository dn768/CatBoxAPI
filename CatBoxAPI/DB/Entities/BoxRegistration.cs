using CatBoxAPI.Enums;

namespace CatBoxAPI.DB.Entities;

// Omitting the Entity suffix and [Table] attribute this time around, since I used a DTO suffix for models/DTOs
// TODO: Probably rename CatProfileEntity to CatProfile or just Cat
public class BoxRegistration
{
    public required Guid Id { get; set; }
    public required CatProfileEntity Cat { get; set; }
    public required string BoxType { get; set; }
    public required BoxSize BoxSize { get; set; }
    public string? SpecialFeatures { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
