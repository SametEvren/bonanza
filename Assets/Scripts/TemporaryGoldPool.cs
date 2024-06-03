using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Temporary Gold Pool")]

public class TemporaryGoldPool : ScriptableObject
{
    public PlayerData playerData;
    public ulong gold;
    public float multiplier;
    public Action<ulong> onTemporaryGoldChange;
    
    public ulong Gold
    {
        get => gold;
        set
        {
            gold = value;
            onTemporaryGoldChange?.Invoke(gold);
        }
    }
    
    public void ApplyToPlayer()
    {
        var oldGold = playerData.Gold;
        var increasedAmount = (ulong)(Gold * Mathf.Max(multiplier, 1));
        playerData.Gold += increasedAmount;
        Debug.Log("Old Gold:" + oldGold 
                              + "Gold To Increase:" + Gold
                              + "Multiplier" + multiplier
                              + "Increase: " + increasedAmount + 
                              "New Gold: " + playerData.Gold);
        Gold = 0;
        multiplier = 0;
    }
}
