using System;
using GameplayControllers;
using PrimeTween;
using SymbolScriptables;
using UnityEngine;
using Utility;

namespace SlotItem
{
    [RequireComponent(typeof(SlotItemView))]
    public class SlotItemController : MonoBehaviour
    {
        public int slotIndexOnReel = -1;
        
        private SlotItemView _slotItemView;
        private SlotItemModel _slotItemModel;

        public SlotItemType SlotItemType => _slotItemModel.slotItemType;
        public SlotItemModel Model => _slotItemModel;
        
        public SymbolData SymbolData => Model.slotSymbolData;

        public SymbolType SymbolType => SymbolData.symbolType;
        
        private IObjectPoolManager<SlotItemController> ObjectPoolManager => 
            ServiceLocator.Get<IObjectPoolManager<SlotItemController>>();
        private WinningsController WinningsController => ServiceLocator.Get<WinningsController>();

        private Tween _moveDownTween;
        private void Awake()
        {
            _slotItemView = GetComponent<SlotItemView>();
        }

        public void SetModel(SlotItemModel slotItemModel)
        {
            _slotItemModel = slotItemModel;
            _slotItemView.Render(_slotItemModel);
        }

        public void DisableSlot()
        {
            _moveDownTween.Stop();
            
            WinningsController.RemoveFromItemCount(SymbolType);
            ObjectPoolManager.ReleaseItemToPool(this);
        }

        public void MoveDownSmoothly(int amountOfSlots, Action onComplete)
        {
            var rectTransform = GetComponent<RectTransform>();
            var anchoredPosition = rectTransform.anchoredPosition;
            var unitHeight = rectTransform.sizeDelta.y;
            var changeAmount = unitHeight * amountOfSlots;
            var newPosition = new Vector2(anchoredPosition.x, anchoredPosition.y - changeAmount);
            _moveDownTween = Tween.UIAnchoredPosition(rectTransform, newPosition, 1f).OnComplete(onComplete);
        }

        public void ResetSlotItem()
        {
            slotIndexOnReel = -1;
        }
    }
}