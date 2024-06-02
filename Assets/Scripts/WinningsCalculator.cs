﻿using System.Collections.Generic;
using SlotItem;
using SymbolScriptables;
using UnityEngine;

public class WinningsCalculator : MonoBehaviour
{
    public ulong betAmount = 10_000_000;
    private const ulong BaseBetAmount = 1_000_000;
    [SerializeField] private PlayerDataController _playerDataController;
    [SerializeField] private UnityEngine.Rendering.SerializedDictionary<SymbolType, int> _slotItemsDictionary = new();

    public void AddToItemCount(SymbolType symbolType)
    {
        if (_slotItemsDictionary.ContainsKey(symbolType))
        {
            _slotItemsDictionary[symbolType]++;
        }
        else
        {
            _slotItemsDictionary.Add(symbolType, 1);
        }
    }

    public void RemoveFromItemCount(SymbolType symbolType)
    {
        if (_slotItemsDictionary.ContainsKey(symbolType))
        {
            if (_slotItemsDictionary[symbolType] > 1)
            {
                _slotItemsDictionary[symbolType]--;
            }
            else
            {
                _slotItemsDictionary.Remove(symbolType);
            }
        }
    }
    private double GetBetMultiplier(ulong betAmount)
    {
        return betAmount / (double)BaseBetAmount;
    }

    public void EarnWinnings(List<CommonSymbolData> data)
    {
        ulong earnings = 0;
        
        foreach (var symbolData in data)
        {
            earnings += symbolData.payoutValue;
        }

        earnings = (ulong)(earnings * GetBetMultiplier(betAmount));
        
        _playerDataController.ChangeGold(earnings);
    }

    public Dictionary<SymbolType, int> GetMatches()
    {
        var itemsToRemove = new Dictionary<SymbolType, int>();
        
        foreach (var entry in _slotItemsDictionary)
        {
            if (entry.Value >= 8)
            {
                itemsToRemove.Add(entry.Key,entry.Value);
            }
        }
        return itemsToRemove;
    }
}