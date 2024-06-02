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

    public SlotItemType EvaluateTypeValue(float referencePoint)
    {
        if (referencePoint <= typeChances.commonChance)
            return SlotItemType.Common;
        
        if (referencePoint - typeChances.commonChance <=  typeChances.scatterChance)
            return SlotItemType.Scatter;

        return SlotItemType.Multiplier;
    }

    public int EvaluateSkinIndex(SlotItemType slotItemType, float referencePoint)
    {
        var skinChances = GetSkinChances(slotItemType);
        if (skinChances == null || skinChances.Value.chances.Count == 0)
        {
            return 0; //Return default skin index.
        }

        var cumulativeChance = 0f;
        for (int i = 0; i < skinChances.Value.chances.Count; i++)
        {
            cumulativeChance += skinChances.Value.chances[i];
            if (referencePoint <= cumulativeChance)
            {
                return i;
            }
        }

        return 0; //Return default skin index.
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