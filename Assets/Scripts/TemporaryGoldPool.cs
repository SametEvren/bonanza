using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Temporary Gold Pool")]
public class TemporaryGoldPool : ScriptableObject
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ulong gold;
    public float multiplier = 1f;

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
        ulong oldGold = playerData.Gold;
        ulong increasedAmount = (ulong)(Gold * Mathf.Max(multiplier, 1f));
        playerData.Gold += increasedAmount;

        Debug.Log($"Old Gold: {oldGold}, Gold To Increase: {Gold}, Multiplier: {multiplier}, Increase: {increasedAmount}, New Gold: {playerData.Gold}");

        Gold = 0;
        multiplier = 1f;
    }
}