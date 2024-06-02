using System;
using System.Collections.Generic;
using SlotItem;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Chances")]
public class SpawnChances : ScriptableObject
{
    public TypeChances typeChances;
    public List<SymbolChanceEntry> symbolChanceEntries;

    public SymbolChances? GetSymbolChances(SlotItemType slotItemType)
    {
        foreach (var symbolChanceEntry in symbolChanceEntries)
        {
            if (symbolChanceEntry.slotItemType == slotItemType)
                return symbolChanceEntry.symbolChances;
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

    public int EvaluateSymbolIndex(SlotItemType slotItemType, float referencePoint)
    {
        var symbolChances = GetSymbolChances(slotItemType);
        if (symbolChances == null || symbolChances.Value.chances.Count == 0)
        {
            return 0; //Return default symbol index.
        }

        var cumulativeChance = 0f;
        for (int i = 0; i < symbolChances.Value.chances.Count; i++)
        {
            cumulativeChance += symbolChances.Value.chances[i];
            if (referencePoint <= cumulativeChance)
            {
                return i;
            }
        }

        return 0; //Return default symbol index.
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
public struct SymbolChances
{
    public List<float> chances;
}

[Serializable]
public struct SymbolChanceEntry
{
    public SlotItemType slotItemType;
    public SymbolChances symbolChances;
}