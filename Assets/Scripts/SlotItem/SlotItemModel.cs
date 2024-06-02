using SymbolScriptables;

namespace SlotItem
{
    public struct SlotItemModel
    {
        public SlotItemType slotItemType;
        public SymbolData slotSymbolData;
        
        public SlotItemModel(SlotItemType slotItemType, SymbolData slotSymbolData)
        {
            this.slotItemType = slotItemType;
            this.slotSymbolData = slotSymbolData;
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