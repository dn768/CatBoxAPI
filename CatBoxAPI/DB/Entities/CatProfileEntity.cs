using CatBoxAPI.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatBoxAPI.DB.Entities;

[Table("CatProfile")]
public class CatProfileEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public int Age { get; set; }
    public required string Color { get; set; }
    public decimal? Weight { get; set; }
    public required string Sex { get; set; }
    public required BoxSize PurrferedBoxSize { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
