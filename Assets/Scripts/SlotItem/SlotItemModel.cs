namespace SlotItem
{
    public struct SlotItemModel
    {
        public SlotItemType slotItemType;
        public int slotSkinIndex;
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