using CatBoxAPI.Enums;

namespace CatBoxAPI.Models.CatProfile;

public class CatProfileCreationDTO
{
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public int Age { get; set; }
    public required string Color { get; set; }
    public decimal? Weight { get; set; }
    public required string Sex { get; set; }
    public required string PurrferedBoxSize { get; set; }

    // TODO: Consider using a custom JsonConverter, or some form of type converter. At least move this to an extension method.
    public BoxSize GetBoxSize()
    {
        if (Enum.TryParse(PurrferedBoxSize, out BoxSize size))
            return size;
        else
            throw new Exception($"PurrferedBoxSize was not a valid enum value: {PurrferedBoxSize}");
    }
}
