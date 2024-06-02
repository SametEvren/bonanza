using UnityEngine;

namespace SlotItem
{
    [RequireComponent(typeof(SlotItemView))]
    public class SlotItemController : MonoBehaviour
    {
        
        private SlotItemView _slotItemView;
        private SlotItemModel _slotItemModel;

        public SlotItemType SlotItemType => _slotItemModel.slotItemType;
        public SlotItemModel Model => _slotItemModel; 
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
            
        }
    }
}