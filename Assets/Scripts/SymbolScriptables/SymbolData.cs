using SlotItem;
using UnityEngine;

namespace SymbolScriptables
{
    public class SymbolData : ScriptableObject
    {
        public SymbolType symbolType;
        public Sprite sprite;
        public float spawnWeight;
        public int minimumMatchCount;
    }
}