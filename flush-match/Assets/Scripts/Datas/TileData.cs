using System.Collections.Generic;

public enum TileType
{
    EMPTY,
    ACE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    NINE,
    TEN,
    JACK,
    QUEEN,
    KING,
    HEART,
    DIAMOND,
    SPADE,
    CLUB
}

public class TilecolorData
{
    private Dictionary<TileType, string> tileColors;

    public TilecolorData()
    {
        tileColors = new Dictionary<TileType, string>()
        {
            { TileType.HEART, "#FF0000" },
            { TileType.DIAMOND, "#00FF00" },
            { TileType.SPADE, "#0000FF" },
            { TileType.CLUB, "#00FfFF" }
        };
    }

    public string GetColor(TileType tileType)
    {
        if (tileColors.TryGetValue(tileType, out string color)) return color;
        return "#FFFFFF";
    }
}