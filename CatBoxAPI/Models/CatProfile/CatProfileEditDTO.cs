namespace CatBoxAPI.Models.CatProfile;

public class CatProfileEditDTO : CatProfileDTOBase
{
    public required Guid Id { get; set; }
    public string? Nickname { get; set; }
    public decimal? Weight { get; set; }
}
