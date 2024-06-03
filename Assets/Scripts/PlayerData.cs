using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private ulong gold;
    [SerializeField] private int freeSpinAmount;
    [SerializeField] private int experience;
    [SerializeField] private int level;
    
    public Action<ulong> onGoldChange;
    public Action<int> onFreeSpinChange;
    public Action<int> onExperienceChange;
    public Action<int> onLevelChange;
    
    public ulong Gold
    {
        get => gold;
        set
        {
            gold = value;
            onGoldChange?.Invoke(gold);
        }
    }

    public int FreeSpinAmount
    {
        get => freeSpinAmount;
        set
        {
            freeSpinAmount = value;
            onFreeSpinChange?.Invoke(freeSpinAmount);
        }
    }
    
    public int Experience
    {
        get => experience;
        set
        {
            experience = value;
            onExperienceChange?.Invoke(experience);
        }
    }

    public int Level
    {
        get => level;
        set
        {
            level = value;
            onLevelChange?.Invoke(level);
        }
    }
    
}