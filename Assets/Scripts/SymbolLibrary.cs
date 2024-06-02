﻿using System.Collections.Generic;
using SlotItem;
using SymbolScriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolLibrary", menuName = "ScriptableObjects/SymbolLibrary")]
public class SymbolLibrary : ScriptableObject
{
    public List<CommonSymbolData> commonSymbolData;
    public List<ScatterSymbolData> scatterSymbolData;
    public List<MultiplierSymbolData> multiplierSymbolData;
    private float _commonSymbolWeightTotal = -1;
    private float _scatterSymbolWeightTotal = -1;
    private float _multiplierSymbolWeightTotal = -1;

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
