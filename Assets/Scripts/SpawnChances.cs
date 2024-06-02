using System;
using SlotItem;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Chances")]
public class SpawnChances : ScriptableObject
{
    public TypeChances typeChances;

    public SlotItemType EvaluateTypeValue(float referencePoint)
    {
        if (referencePoint <= typeChances.commonChance)
            return SlotItemType.Common;
        
        if (referencePoint - typeChances.commonChance <=  typeChances.scatterChance)
            return SlotItemType.Scatter;

        return SlotItemType.Multiplier;
    }
}

[Serializable]
public struct TypeChances 
{ 
    public float commonChance;
    public float scatterChance;
    public float multiplierChance;
}
