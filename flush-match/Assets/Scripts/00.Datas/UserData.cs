[System.Serializable]
public class UserData
{
    public string playerName;
    public int bestScore;
    public int bestStage;

    public UserData(string playerName, int bestScore, int bestStage)
    {
        this.playerName = playerName;
        this.bestScore = bestScore;
        this.bestStage = bestStage;
    }
}
