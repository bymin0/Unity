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
    public const string DefaultPlayName = "����";
    public const string BestScore = "�ְ� ���� : ";
    public const string BestStage = "�ְ� �������� : ";
    public const string Slash = " / ";
    public const string Slot = "����";

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
    public const string ShuffleName = "����";
    public const string TimerName = "Ÿ�̸�";
    public const string AutoName = "�ڵ� ���߱�";
    public const string JokerName = "����";
    public const string Combo1Name = "�޺� 1�� ����";
    public const string Combo2Name = "�޺� 2�� ����";
    public const string JumpLevleName = "���� �ǳʶٱ�";
    public const string FrozenName = "Ÿ�̸� ���߱�";
    public const string ShuffleCmt = "��ü Ÿ���� �����ϰ� �����ִ� ������.";
    public const string TimerCmt = "Ÿ�̸Ӹ� 20�� �÷��ִ� ������.\n�ð��� �ִ� �ð��� �ѱ� �� �����ϴ�.";
    public const string AutoCmt = "������ Ÿ�� �� ���� �����ִ� ������.";
    public const string JokerCmt = "������ Ÿ�� ������ ��� ����� �ٲٴ� ������.\n���� Ÿ���� �ٷ� �������ϴ�.";
    public const string AddCombo1Cmt = "�޺� �ð� 1�� ����\n(�ִ� 5�ʱ���\n���� ����)";
    public const string AddCombo2Cmt = "�޺� �ð� 2�� ����\n(�ִ� 5�ʱ���\n���� ����)";
    public const string JumpLevelCmt = "���� ���� �ǳʶٱ�";
    public const string FrozenCmt = "5�ʰ� Ÿ�̸� ���߱�";
    public const string AutoRemoveCmt = "������ Ÿ�� �� ��\n���ֱ�";
    public const string BuyItem = "BuyItem";
    public const string BuyAlert = "��(��)\n�����߽��ϴ�.";
    public const string IconContainer = "IconContainer/";
    public const string Icon = "Icon";
    public const string Price = "Price";
    public const string Coin = "����";
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