using CatBoxAPI.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatBoxAPI.DB.Entities;

// TODO: Probably rename CatProfileEntity to CatProfile or just Cat
[Table(nameof(BoxRegistration))]
public class BoxRegistration
{
    public required Guid Id { get; set; }
    public required CatProfileEntity Cat { get; set; }
    public required string BoxType { get; set; }
    public required BoxSize BoxSize { get; set; }
    public string? SpecialFeatures { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool? IsApproved { get; set; } = false;
    public string? DecisionReason { get; set; }
    public DateTime? DecidedOn { get; set; }
}
