using SymbolScriptables;

namespace SlotItem
{
    public struct SlotItemModel
    {
        public SlotItemType slotItemType;
        public SymbolData slotSymbolData;
        public float multiplierAmount;
        
        public SlotItemModel(SlotItemType slotItemType, SymbolData slotSymbolData, float multiplierAmount)
        {
            this.slotItemType = slotItemType;
            this.slotSymbolData = slotSymbolData;
            this.multiplierAmount = multiplierAmount;
        }
    }

    public enum SlotItemType
    {
        Common,
        Scatter,
        Multiplier
    }

    public enum SymbolType
    {
        Cannon,
        Compass,
        Hook,
        ManPirate,
        Map,
        Parrot,
        Rum,
        Skull,
        WomanPirate,
        Multiplier,
        Scatter
    }
}