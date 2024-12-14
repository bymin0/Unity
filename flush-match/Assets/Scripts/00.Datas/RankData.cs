using System.Collections.Generic;

[System.Serializable]
public class RankData
{
    public string playerName;
    public int stage;
    public int score;

    public RankData(string playerName, int stage, int score)
    {
        this.playerName = playerName;
        this.stage = stage;
        this.score = score;
    }
}

[System.Serializable]
public class RankList
{
    public List<RankData> ranks = new List<RankData>();
}
