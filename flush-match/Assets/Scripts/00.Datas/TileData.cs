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
    JOKER,
    MAX,
}

public class TileData
{
    //private Dictionary<TileType, string> tileColors;
    private Dictionary<TileType, string> tileSprite;

    public TileData()
    {
        /*tileColors = new Dictionary<TileType, string>()
        {
            { TileType.ACE, "#4E0F1D" },
            { TileType.TWO, "#7EDF90" },
            { TileType.THREE, "#F012FF" },
            { TileType.FOUR, "#90F218" },
            { TileType.FIVE, "#AAAAAA" },
            { TileType.SIX, "#EdA8F0" },
            { TileType.SEVEN, "#DD2D88" },
            { TileType.NINE, "#222222" },
            { TileType.TEN, "#123456" },
            { TileType.JACK, "#AEFFCD" },
            { TileType.QUEEN, "#000000" },
            { TileType.KING, "#510963" },
            { TileType.HEART, "#FF0000" },
            { TileType.DIAMOND, "#00FF00" },
            { TileType.SPADE, "#0000FF" },
            { TileType.CLUB, "#00FFFF" },
            { TileType.JOKER, "#FA5882" }
        };*/
        tileSprite = new Dictionary<TileType, string>()
        {
            { TileType.ACE, ConstCollect.ACE },
            { TileType.TWO, ConstCollect.TWO },
            { TileType.THREE, ConstCollect.THREE },
            { TileType.FOUR, ConstCollect.FOUR },
            { TileType.FIVE, ConstCollect.FIVE },
            { TileType.SIX, ConstCollect.SIX },
            { TileType.SEVEN, ConstCollect.SEVEN },
            { TileType.EIGHT, ConstCollect.EIGHT },
            { TileType.NINE, ConstCollect.NINE },
            { TileType.TEN, ConstCollect.TEN },
            { TileType.JACK, ConstCollect.JACK },
            { TileType.QUEEN, ConstCollect.QUEEN },
            { TileType.KING, ConstCollect.KING },
            { TileType.HEART, ConstCollect.HEART },
            { TileType.DIAMOND, ConstCollect.DIAMOND },
            { TileType.SPADE, ConstCollect.SPADE },
            { TileType.CLUB, ConstCollect.CLUB },
            { TileType.JOKER, ConstCollect.JOKER }
        };
    }

    public string GetSprite(TileType tileType)
    {
        if (tileSprite.TryGetValue(tileType, out string path)) return path;
        return null;
    }

    /*public string GetColor(TileType tileType)
    {
        if (tileColors.TryGetValue(tileType, out string color)) return color;
        return "#FFFFFF";
    }*/
}