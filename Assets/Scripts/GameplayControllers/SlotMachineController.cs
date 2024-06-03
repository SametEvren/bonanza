using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reel;
using SlotItem;
using SymbolScriptables;
using UnityEngine;
using Utility;

namespace GameplayControllers
{
    public class SlotMachineController : MonoBehaviour
    {
        public List<SlotItemController> AllItemControllers
        {
            get
            {
                var controllers = new List<SlotItemController>();
                foreach (var reelController in reelControllers)
                {
                    controllers.AddRange(reelController.slotItemControllers);
                }

                return controllers;
            }
        }

        [SerializeField] private SymbolLibrary symbolLibrary;
        [SerializeField] private List<ReelController> reelControllers;
        [SerializeField] private WinningsController winningsController;
        [SerializeField] private BetController betController;
        [SerializeField] private FreeSpinController freeSpinController;
        [SerializeField] private TemporaryGoldPool temporaryGoldPool;

        private SymbolSpawner SymbolSpawner => ServiceLocator.Get<SymbolSpawner>();

        private int spinningSlotsCount = 0;
        private bool isSpinCompleteHandling = false;

        private void Start()
        {
            StartCoroutine(SpinTheSlot());
        }

        public void TrySpinningTheSlot()
        {
            if (freeSpinController.TrySpinningForFree())
            {
                StartCoroutine(SpinTheSlot());
            }
            else if (betController.MakeBet())
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
                spinningSlotsCount++;
                var slotIndex = i;

                spawnedItem.MoveDownSmoothly(ReelController.ReelSlotCapacity, () =>
                {
                    spinningSlotsCount--;
                    spawnedItem.slotIndexOnReel = slotIndex;
                    if (spinningSlotsCount == 0)
                        HandleSpinComplete();
                });
            }
        }

        private void HandleSpinComplete()
        {
            if (isSpinCompleteHandling) return;

            Debug.Log("Handle Spin Complete");
            isSpinCompleteHandling = true;

            var matches = winningsController.GetMatches();
            var symbolsToRemove = matches.Keys.ToList();

            var multiplierSymbols = new List<SlotItemController>();

            var containsNonMultiplierItems = symbolsToRemove.Any(s => s != SymbolType.Multiplier);

            symbolsToRemove.Remove(SymbolType.Multiplier);

            foreach (var reelController in reelControllers)
            {
                var amountToSpawn = reelController.RemoveSymbolsOfType(symbolsToRemove);
                if (amountToSpawn > 0)
                {
                    SymbolSpawner.SpawnOnTop(reelController, amountToSpawn);
                    reelController.MoveItemsDown(amountToSpawn, HandleSpinComplete);
                }
            }

            var finalMultiplier = 0f;

            if (!containsNonMultiplierItems)
            {
                multiplierSymbols.AddRange(AllItemControllers.Where(s => s.SymbolType == SymbolType.Multiplier));
                foreach (var multiplierSymbol in multiplierSymbols)
                {
                    multiplierSymbol.PickMultiplierAmount();
                    finalMultiplier += multiplierSymbol.Model.multiplierAmount;
                }

                Debug.Log("Final Multiplier: " + finalMultiplier);

                StartCoroutine(DelayedRemove(symbolsToRemove));
            }

            var commonSymbols = symbolLibrary.PickCommonData(symbolsToRemove);

            if (symbolsToRemove.Contains(SymbolType.Scatter))
            {
                freeSpinController.TriggerFreeSpinGain();
            }

            var tuples = new List<(CommonSymbolData symbolData, int amount)>();

            foreach (var commonSymbol in commonSymbols)
            {
                tuples.Add((commonSymbol, matches[commonSymbol.symbolType]));
            }

            winningsController.EarnWinnings(tuples, finalMultiplier);

            if (symbolsToRemove.Count == 0 || !containsNonMultiplierItems)
            {
                StartCoroutine(ApplyWithinDelay());
            }
            else
            {
                isSpinCompleteHandling = false;
            }
        }

        private IEnumerator ApplyWithinDelay()
        {
            yield return new WaitForSeconds(1f);
            temporaryGoldPool.ApplyToPlayer();
            isSpinCompleteHandling = false;
        }

        private IEnumerator DelayedRemove(List<SymbolType> symbolsToRemove)
        {
            yield return new WaitForSeconds(0.75f);
            foreach (var reelController in reelControllers)
            {
                var amountToSpawn = reelController.RemoveSymbolsOfType(symbolsToRemove);
                if (amountToSpawn > 0)
                {
                    SymbolSpawner.SpawnOnTop(reelController, amountToSpawn);
                    reelController.MoveItemsDown(amountToSpawn, HandleSpinComplete);
                }
            }
            isSpinCompleteHandling = false;
        }

        private void HandleSpinFailed()
        {
            Debug.LogError("Spin failed due to insufficient funds or no free spins available.");
        }
    }
}
