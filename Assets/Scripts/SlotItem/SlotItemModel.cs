using System;

namespace SlotItem
{
    public struct SlotItemModel : IEquatable<SlotItemModel>
    {
        public SlotItemType slotItemType;
        public int slotSkinIndex;

        public SlotItemModel(SlotItemType slotItemType, int slotSkinIndex)
        {
            this.slotItemType = slotItemType;
            this.slotSkinIndex = slotSkinIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is SlotItemModel model && Equals(model);
        }

        public bool Equals(SlotItemModel other)
        {
            return slotItemType == other.slotItemType && slotSkinIndex == other.slotSkinIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 29 + slotItemType.GetHashCode();
            hashCode = hashCode * 29 + slotSkinIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SlotItemModel left, SlotItemModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SlotItemModel left, SlotItemModel right)
        {
            return !(left == right);
        }
    }

    public enum SlotItemType
    {
        Common,
        Scatter,
        Multiplier
    }

    public enum CommonSlotItemType
    {
        Cannon,
        Compass,
        Hook,
        ManPirate,
        Map,
        Parrot,
        Rum,
        Skull,
        WomanPirate
    }

    public enum MultiplierSlotItemType
    {
        CrossSwords
        // This is an enum for future expandability in case we need more skins.
    }

    public enum ScatterSlotItemType
    {
        SkullInChest
        // This is an enum for future expandability in case we need more skins.
    }
}