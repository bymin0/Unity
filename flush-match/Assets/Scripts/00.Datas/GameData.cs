using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static Dictionary<int, UserData> SlotData = new Dictionary<int, UserData>();

    public static UserData CurrentUserData {  get; private set; }
    public static int CurrentSlotNumber { get; private set; }

    public static void SetCurrentUserData(int slotNumber)
    {
        if (SlotData.ContainsKey(slotNumber))
        {
            CurrentSlotNumber = slotNumber;
            CurrentUserData = SlotData[slotNumber];
        }
    }

    public static void SaveSlotData(int slotNumber, UserData userData)
    {
        SlotData[slotNumber] = userData;
    }
}