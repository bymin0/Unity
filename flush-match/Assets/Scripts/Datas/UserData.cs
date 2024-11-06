[System.Serializable]
public class UserData
{
    public string name;
    public int level;
    public ItemCount itemCnt;

    [System.Serializable]
    public class ItemCount
    {
        public int ShuffleCnt;
        public int TimerCnt;
        public int JokerCnt;
        public int AutoCnt;
    }
}
