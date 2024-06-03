using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reel;
using SlotItem;
using SymbolScriptables;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace GameplayControllers
{
    public class SlotMachineController : MonoBehaviour
    {
        [SerializeField] private SymbolLibrary symbolLibrary;
        [SerializeField] private List<ReelController> reelControllers;
        [FormerlySerializedAs("winningsCalculator")] [SerializeField] private WinningsController winningsController;
        [SerializeField] private BetController betController;
        [SerializeField] private FreeSpinController freeSpinController;
    
        private SymbolSpawner SymbolSpawner => ServiceLocator.Get<SymbolSpawner>();

        private int _spinningSlots = 0;
    
        private void Start()
        {
            StartCoroutine(SpinTheSlot());
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                TrySpinningTheSlot();
            }
        }
    
        public void TrySpinningTheSlot()
        {
            if (freeSpinController.TrySpinningForFree() || betController.MakeBet())
            {
                StartCoroutine(SpinTheSlot());
            }
            else
            {
                HandleSpinFailed();
            }
        }
    
        private IEnumerator SpinTheSlot()
        {
            foreach (var reelController in reelControllers)
            {
                SpinReelAround(reelController);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void SpinReelAround(ReelController reelController)
        {
            var oldItems = new List<SlotItemController>(reelController.slotItemControllers);
            var spawnedItems = SymbolSpawner.SpawnOnTop(reelController, ReelController.ReelSlotCapacity);

            foreach (var oldItem in oldItems)
            {
                oldItem.MoveDownSmoothly(ReelController.ReelSlotCapacity, () =>
                {
                    reelController.RemoveSlotItem(oldItem);
                });
            }

            for (var i = 0; i < spawnedItems.Count; i++)
            {
                var spawnedItem = spawnedItems[i];
                _spinningSlots++;
                var slotIndex = i;
            
                spawnedItem.MoveDownSmoothly(ReelController.ReelSlotCapacity, () =>
                {
                    _spinningSlots--;
                    if (_spinningSlots == 0)
                        HandleSpinComplete();
                    spawnedItem.slotIndexOnReel = slotIndex;
                });
            }
        }

        private void HandleSpinComplete()
        {
            var matches = winningsController.GetMatches();
            var symbolsToRemove = matches.Keys.ToList();

            foreach (var reelController in reelControllers)
            {
                var amountToSpawn = reelController.RemoveSymbolsOfType(symbolsToRemove);
                if (amountToSpawn > 0)
                {            
                    SymbolSpawner.SpawnOnTop(reelController, amountToSpawn);
                    reelController.MoveItemsDown(amountToSpawn, HandleSpinComplete);
                }
            }

            var commonSymbols = symbolLibrary.PickCommonData(symbolsToRemove);

            if (symbolsToRemove.Contains(SymbolType.Scatter))
            {
                freeSpinController.TriggerFreeSpinGain();
            }
            
            
            var tuples = new List<(CommonSymbolData symbolData, int amount)>();

            foreach (var commonSymbol in commonSymbols)
            {
                tuples.Add((commonSymbol,matches[commonSymbol.symbolType]));
            }
        
            winningsController.EarnWinnings(tuples);
        }


        private void HandleSpinFailed()
        {
            throw new System.NotImplementedException();
        }
    }
}
