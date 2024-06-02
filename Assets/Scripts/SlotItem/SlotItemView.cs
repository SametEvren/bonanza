using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotItem
{
    [RequireComponent(typeof(Image))]
    public class SlotItemView : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Render(SlotItemModel slotItemModel)
        {
            gameObject.SetActive(true);
            
            
            _image.sprite = slotItemModel.slotSymbolData.sprite;
        }
    }
}