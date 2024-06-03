using System;
using System.Collections.Generic;
using SlotItem;
using SymbolScriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolLibrary", menuName = "ScriptableObjects/SymbolLibrary")]
public class SymbolLibrary : ScriptableObject
{
    public List<CommonSymbolData> commonSymbolData;
    public List<ScatterSymbolData> scatterSymbolData;
    public List<MultiplierSymbolData> multiplierSymbolData;

    public int commonMatchMinimum;
    public int scatterMatchMinimum;
    public int multiplierMatchMinimum;
    
    private float _commonSymbolWeightTotal = -1;
    private float _scatterSymbolWeightTotal = -1;
    private float _multiplierSymbolWeightTotal = -1;

    public SlotItemType GetType(SymbolType symbolType)
    {
        foreach (var commonSymbol in commonSymbolData)
        {
            if (commonSymbol.symbolType == symbolType)
                return SlotItemType.Common;
        }
        
        foreach (var scatterSymbol in scatterSymbolData)
        {
            if (scatterSymbol.symbolType == symbolType)
                return SlotItemType.Scatter;
        }
        
        foreach (var multiplierSymbol in multiplierSymbolData)
        {
            if (multiplierSymbol.symbolType == symbolType)
                return SlotItemType.Multiplier;
        }

        return SlotItemType.Common;;
    }

    public int GetMinimumMatchForType(SlotItemType slotItemType)
    {
        switch (slotItemType)
        {
            case SlotItemType.Common:
                return commonMatchMinimum;
            case SlotItemType.Scatter:
                return scatterMatchMinimum;
            case SlotItemType.Multiplier:
                return multiplierMatchMinimum;
        }

        return commonMatchMinimum;
    }

    public List<CommonSymbolData> PickCommonData(List<SymbolType> symbolTypes)
    {
        List<CommonSymbolData> selectedData = new List<CommonSymbolData>();

        foreach (var symbolType in symbolTypes)
        {
            foreach (var symbolData in commonSymbolData)
            {
                if (symbolData.symbolType == symbolType)
                {
                    selectedData.Add(symbolData);
                    break;
                } 
            }
        }

        return selectedData;
    }
    
    private void CalculateTotalWeights()
    {
        if (_commonSymbolWeightTotal < 0)
        {
            _commonSymbolWeightTotal = 0;
            foreach (var symbol in commonSymbolData)
            {
                _commonSymbolWeightTotal += symbol.spawnWeight;
            }
        }

        if (_scatterSymbolWeightTotal < 0)
        {
            _scatterSymbolWeightTotal = 0;
            foreach (var symbol in scatterSymbolData)
            {
                _scatterSymbolWeightTotal += symbol.spawnWeight;
            }
        }

        if (_multiplierSymbolWeightTotal < 0)
        {
            _multiplierSymbolWeightTotal = 0;
            foreach (var symbol in multiplierSymbolData)
            {
                _multiplierSymbolWeightTotal += symbol.spawnWeight;
            }
        }
    }

    public SymbolData GetTargetSymbol(SlotItemType slotType, float rand)
    {
        CalculateTotalWeights();

        switch (slotType)
        {
            case SlotItemType.Common:
                return GetRandomSymbol(commonSymbolData, _commonSymbolWeightTotal, rand);
            case SlotItemType.Scatter:
                return GetRandomSymbol(scatterSymbolData, _scatterSymbolWeightTotal, rand);
            case SlotItemType.Multiplier:
                return GetRandomSymbol(multiplierSymbolData, _multiplierSymbolWeightTotal, rand);
            default:
                return null;
        }
    }

    private SymbolData GetRandomSymbol<T>(List<T> symbolDataList, float totalWeight, float rand) where T : SymbolData
    {
        float cumulativeWeight = 0;
        float targetWeight = rand * totalWeight;

        foreach (var symbol in symbolDataList)
        {
            cumulativeWeight += symbol.spawnWeight;
            if (targetWeight <= cumulativeWeight)
            {
                return symbol;
            }
        }

        if (symbolDataList.Count > 0)
        {
            return symbolDataList[0];
        }

        return null; // Default case, should not be reached
    }
}
