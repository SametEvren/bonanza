using System;
using System.Collections.Generic;
using SlotItem;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Chances")]
public class SpawnChances : ScriptableObject
{
    public TypeChances typeChances;
    public List<SkinChanceEntry> skinChanceEntries;

    public SkinChances? GetSkinChances(SlotItemType slotItemType)
    {
        foreach (var skinChanceEntry in skinChanceEntries)
        {
            if (skinChanceEntry.slotItemType == slotItemType)
                return skinChanceEntry.skinChances;
        }

        return null;
    }
}

[Serializable]
public struct TypeChances 
{ 
    public float commonChance;
    public float scatterChance;
    public float multiplierChance;
}

[Serializable]
public struct SkinChances
{
    public List<float> chances;
}

[Serializable]
public struct SkinChanceEntry
{
    public SlotItemType slotItemType;
    public SkinChances skinChances;
}