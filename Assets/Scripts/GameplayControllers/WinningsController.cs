using System.Collections.Generic;
using SlotItem;
using SymbolScriptables;
using UnityEngine;
using Utility;

namespace GameplayControllers
{
    public class WinningsController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private SymbolLibrary symbolLibrary;
        [SerializeField] private BetController betController;
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

        public void EarnWinnings(List<(CommonSymbolData symbolData, int amount)> dataTuples)
        {
            double earningMultiplier = 0;
        
            foreach (var tuple in dataTuples)
            {
                var amount = (ulong)tuple.amount;
                var payout = tuple.symbolData.payoutMultiplier;
                earningMultiplier += (amount * payout);
            }

            var earnings = (ulong)(earningMultiplier * betController.CurrentBetAmount);
        
            Debug.Log(earnings);
            playerData.Gold += earnings;
        }

        public Dictionary<SymbolType, int> GetMatches()
        {
            var itemsToRemove = new Dictionary<SymbolType, int>();
        
            foreach (var entry in _slotItemsDictionary)
            {
                if (entry.Value >= symbolLibrary.GetMinimumMatchForType(symbolLibrary.GetType(entry.Key)))
                {
                    itemsToRemove.Add(entry.Key,entry.Value);
                }
            }
            return itemsToRemove;
        }
    }
}