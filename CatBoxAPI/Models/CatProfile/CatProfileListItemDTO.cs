namespace CatBoxAPI.Models.CatProfile;

public class CatProfileListItemDTO
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public int Age { get; set; }
    public required string Color { get; set; }
    public decimal? Weight { get; set; }
    public required string Sex { get; set; }
    public required string PurrferedBoxSize { get; set; }
}
