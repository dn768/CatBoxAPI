using CatBoxAPI.Enums;

namespace CatBoxAPI.Extensions;

public static class StringExtensions
{
    public static BoxSize GetBoxSize(this string boxSize)
    {
        if (Enum.TryParse(boxSize, true, out BoxSize size))
            return size;
        else
            throw new Exception($"PurrferedBoxSize was not a valid enum value: {boxSize}");
    }
}
