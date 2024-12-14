using Unity.VisualScripting;
using UnityEngine;

public static class ConstCollect
{
    // Scene Name
    public const string StartScene = "Start";
    public const string SaveLoad = "LoadNSave";
    public const string Game = "Game";
    public const string Shop = "Shop";
    public const string End = "End";

    // Wait time
    public static readonly WaitForSeconds waitFor5ms = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds waitFor7ms = new WaitForSeconds(0.7f);

    // DataFormat
    public const string K = "N0";

    // SaveLoad
    public const string DefaultPlayName = "피코";
    public const string BestScore = "최고 점수 : ";
    public const string BestStage = "최고 스테이지 : ";
    public const string Slash = " / ";
    public const string Slot = "슬롯";

    // Item
    public const string Shuffle = "Shuffle";
    public const string Timer = "Timer";
    public const string Auto = "Auto";
    public const string Joker = "Joker";
    public const string Combo1 = "AddComboTime";
    public const string Combo2 = "Add2ComboTime";
    public const string JumpLevel = "GoNextLevel";
    public const string FrozenTimer = "FrozenTimer";
    public const string AutoRemove = "AutoRemove";

    // Shop
    public const int ErrIdx = 0;
    public const int BuyIdx = 1;
    public const int ReDrawPrice = 50;
    public const float Add1s = 1f;
    public const float Add2s = 2f;
    public const string ShuffleName = "셔플";
    public const string TimerName = "타이머";
    public const string AutoName = "자동 맞추기";
    public const string JokerName = "마녀";
    public const string Combo1Name = "콤보 1초 증가";
    public const string Combo2Name = "콤보 2초 증가";
    public const string JumpLevleName = "레벨 건너뛰기";
    public const string FrozenName = "타이머 멈추기";
    public const string ShuffleCmt = "전체 타일을 랜덤하게 섞어주는 아이템.";
    public const string TimerCmt = "타이머를 20초 늘려주는 아이템.\n시간은 최대 시간을 넘길 수 없습니다.";
    public const string AutoCmt = "랜덤한 타일 한 쌍을 없애주는 아이템.";
    public const string JokerCmt = "랜텀한 타일 종류를 모두 마녀로 바꾸는 아이템.\n마녀 타일은 바로 없어집니다.";
    public const string AddCombo1Cmt = "콤보 시간 1초 증가\n(최대 5초까지\n누적 가능)";
    public const string AddCombo2Cmt = "콤보 시간 2초 증가\n(최대 5초까지\n누적 가능)";
    public const string JumpLevelCmt = "다음 레벨 건너뛰기";
    public const string FrozenCmt = "5초간 타이머 멈추기";
    public const string AutoRemoveCmt = "랜덤한 타일 한 쌍\n없애기";
    public const string BuyItem = "BuyItem";
    public const string BuyAlert = "을(를)\n구매했습니다.";
    public const string IconContainer = "IconContainer/";
    public const string Icon = "Icon";
    public const string Price = "Price";
    public const string Coin = "코인";
    public const string Description = "Description";

    // Tile
    public const float tileWidth = 1f;
    public const float tileHeight = 1f;
    public const string ACE = "Cakes/Cake1";
    public const string TWO = "Cakes/Cake2";
    public const string THREE = "Cakes/Cake3";
    public const string FOUR = "Cakes/Cake4";
    public const string FIVE = "Cakes/Cake5";
    public const string SIX = "Cakes/Cake6";
    public const string SEVEN = "Cakes/Cake7";
    public const string EIGHT = "Cakes/Cake8";
    public const string NINE = "Cakes/Cake9";
    public const string TEN = "Cakes/Cake10";
    public const string JACK = "Cakes/Cake11";
    public const string QUEEN = "Cakes/Cake12";
    public const string KING = "Cakes/Cake13";
    public const string HEART = "Cakes/Cake14";
    public const string DIAMOND = "Cakes/Cake15";
    public const string SPADE = "Cakes/Cake16";
    public const string CLUB = "Cakes/Cake17";
    public const string JOKER = "Cakes/Cake18";
}