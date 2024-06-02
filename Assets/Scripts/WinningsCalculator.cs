using System;
using System.Collections.Generic;
using SlotItem;
using SymbolScriptables;
using UnityEngine;
using Utility;

public class WinningsCalculator : MonoBehaviour
{
    private const ulong BaseBetAmount = 1_000_000;
    [SerializeField] private BetController betController;
    [SerializeField] private PlayerDataController _playerDataController;
    [SerializeField] private UnityEngine.Rendering.SerializedDictionary<SymbolType, int> _slotItemsDictionary = new();
    private SymbolSpawner SymbolSpawner => ServiceLocator.Get<SymbolSpawner>();

    private void Awake()
    {
        ServiceLocator.Add(this);
    }

    private void Start()
    {
        SymbolSpawner.onSpawnedItem += HandleItemSpawned;
    }

    private void HandleItemSpawned(SymbolType symbolType)
    {
        AddToItemCount(symbolType);
    }

    private void AddToItemCount(SymbolType symbolType)
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

    public void EarnWinnings(List<(CommonSymbolData symbolData, int amount)> dataTuples)
    {
        ulong earnings = 0;
        
        foreach (var tuple in dataTuples)
        {
            var amount = (ulong)tuple.amount;
            var payout = tuple.symbolData.payoutValue;
            earnings += amount * payout;
        }

        earnings = (ulong)(earnings * GetBetMultiplier(betController.CurrentBetAmount));
        
        Debug.Log(earnings);
        _playerDataController.ChangeGold(earnings);
    }

    public void ResetItemCounts()
    {
        _slotItemsDictionary.Clear();
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