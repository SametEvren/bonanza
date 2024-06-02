using System.Collections.Generic;
using SlotItem;
using UnityEngine;

namespace Reel
{
    public class ReelController : MonoBehaviour
    {
        public bool[] activeSlotItems = new bool[5];
        public List<SlotItemController> slotItemControllers;
        
        //Spawn Item
        //Delete Item => Spawn + Move
        //Move Items
    }
}
