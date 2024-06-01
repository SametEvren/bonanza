using System.Collections.Generic;
using UnityEngine;

namespace SlotItem
{
    [CreateAssetMenu(fileName = "New Slot Sprite Library", menuName = " Slot Sprite Library")]
    public class SlotSpriteLibrary : ScriptableObject
    {
        public List<SlotSpriteEntry> slotSpriteEntries;

        public Sprite GetSprite(SlotItemType slotItemType, int index)
        {
            foreach (var slotSpriteEntry in slotSpriteEntries)
            {
                if (slotSpriteEntry.type == slotItemType && slotSpriteEntry.index == index)
                {
                    Sprite sprite = slotSpriteEntry.sprite;
                    return sprite;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class SlotSpriteEntry
    {
        public SlotItemType type;
        public int index;
        public Sprite sprite;
    }
}