using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private ulong gold;

    public ulong Gold
    {
        get => gold;
        set
        {
            gold = value;
            onGoldChange?.Invoke(gold);
        }
    }

    public int experience;
    public int level;

    public Action<ulong> onGoldChange;
}