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
    CLUB,
    JOKER
}

public class TileData
{
    private Dictionary<TileType, string> tileSprites;
    private Dictionary<TileType, string> tileColors;

    public TileData()
    {
        tileColors = new Dictionary<TileType, string>()
        {
            { TileType.HEART, "#FF0000" },
            { TileType.DIAMOND, "#00FF00" },
            { TileType.SPADE, "#0000FF" },
            { TileType.CLUB, "#00FFFF" },
            { TileType.JOKER, "#FA5882" }
        };

        tileSprites = new Dictionary<TileType, string>()
        {
            {TileType.HEART, "Sprites/Aheart" },
            {TileType.DIAMOND, "Sprites/Adiamond" },
            {TileType.SPADE, "Sprites/Aspade" },
            {TileType.CLUB, "Sprites/Aclub" }
        };
    }

    public string GetColor(TileType tileType)
    {
        if (tileColors.TryGetValue(tileType, out string color)) return color;
        return "#FFFFFF";
    }

    public string GetImagePath(TileType tileType)
    {
        if (tileSprites.TryGetValue(tileType, out string imagePath))
            return imagePath;
        return null;
    }
}