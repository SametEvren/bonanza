using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotItem
{
    [RequireComponent(typeof(Image))]
    public class SlotItemView : MonoBehaviour
    {
        private Image _image;
        [SerializeField] private SlotSpriteLibrary slotSpriteLibrary;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Render(SlotItemModel slotItemModel)
        {
            gameObject.SetActive(true);
            var sprite = slotSpriteLibrary.GetSprite(slotItemModel.slotItemType, slotItemModel.slotSkinIndex);
            
            if (sprite is null)
                return;
            
            _image.sprite = sprite;
        }
    }
}