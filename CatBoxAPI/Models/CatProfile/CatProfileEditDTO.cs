namespace CatBoxAPI.Models.CatProfile;

public class CatProfileEditDTO
{
    public required Guid Id { get; set; }
    public string? Nickname { get; set; }
    public decimal? Weight { get; set; }
    public required string PurrferedBoxSize { get; set; }
}
