using CatBoxAPI.Enums;

namespace CatBoxAPI.Extensions;

public static class StringExtensions
{
    public static BoxSize GetBoxSize(this string boxSize)
    {
        if (Enum.TryParse(boxSize, true, out BoxSize size))
            return size;
        else
            throw new UserFriendlyException($"Box size was not a valid enum value: {boxSize}. Box size must be one of these values: {string.Join(", ", Enum.GetValues<BoxSize>())}");
    }
}
