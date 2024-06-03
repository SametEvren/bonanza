using System.Collections.Generic;
using UnityEngine;

namespace SymbolScriptables
{
    [CreateAssetMenu(fileName = "Multiplier Symbol Data")]
    public class MultiplierSymbolData : SymbolData
    {
        public List<float> multiplierAmounts;

        public float PickRandom()
        {
            var rand = Random.Range(0, multiplierAmounts.Count);
            return multiplierAmounts[rand];
        }
    }
}