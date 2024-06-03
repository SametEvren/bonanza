using UnityEngine;

namespace GameplayControllers
{
    public class FreeSpinController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private int gainAmount;
        
        public int FreeSpins
        {
            get => playerData.FreeSpinAmount;
            set => playerData.FreeSpinAmount = value;
        }

        public bool IsFreeSpinActive => FreeSpins > 0;

        public bool TrySpinningForFree()
        {
            if (!IsFreeSpinActive)
                return false;

            FreeSpins--;
            return true;
        }

        public void TriggerFreeSpinGain()
        {
            //TODO: Trigger Free Spin gain UI.
            FreeSpins += gainAmount;
        }
    }
}