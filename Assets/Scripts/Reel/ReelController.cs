using System;
using System.Collections.Generic;
using SlotItem;
using UnityEngine;

namespace Reel
{
    public class ReelController : MonoBehaviour
    {
        public List<SlotItemController> slotItemControllers;
        public const int ReelSlotCapacity = 5;
        private int _movingItems;

        public void MoveItemsDown(int removeAmount, Action onComplete)
        {
            var slotItemControllersCopy = new List<SlotItemController>(slotItemControllers);

            for (var i = 0; i < slotItemControllersCopy.Count; i++)
            {
                var slotItemController = slotItemControllersCopy[i];
                var slotIndex = slotItemController.slotIndexOnReel;

                if (slotIndex < 0)
                {
                    var filledAmount = ReelSlotCapacity - removeAmount;
                    slotIndex = ReelSlotCapacity + i - filledAmount;
                }

                var targetIndex = Mathf.Clamp(i, 0, ReelSlotCapacity - 1);

                if (slotIndex < 0)
                {
                    slotIndex = targetIndex;
                }

                var amountToMoveDown = slotIndex - targetIndex;

                slotItemController.slotIndexOnReel = targetIndex;

                _movingItems++;
                slotItemController.MoveDownSmoothly(amountToMoveDown,
                    () =>
                    {
                        _movingItems--;
                        if (_movingItems == 0)
                        {
                            onComplete?.Invoke();
                        }
                    });
            }
        }

        public int RemoveSymbolsOfType(List<SymbolType> symbolsToRemove)
        {
            var removeAmount = 0;
            var itemsToRemove = new List<SlotItemController>();

            foreach (var slotItemController in slotItemControllers)
            {
                if (symbolsToRemove.Contains(slotItemController.SymbolType))
                {
                    removeAmount++;
                    itemsToRemove.Add(slotItemController);
                }
            }

            
            foreach (var item in itemsToRemove)
            {
                RemoveSlotItem(item);
            }

            return removeAmount;
        }

        public void RemoveSlotItem(SlotItemController slotItemController)
        {
            slotItemControllers.Remove(slotItemController);
            slotItemController.DisableSlot();
        }
    }
}
