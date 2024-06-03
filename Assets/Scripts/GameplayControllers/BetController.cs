using System;
using UnityEngine;

namespace GameplayControllers
{
    public class BetController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        private ulong betChangeAmount = 1_000_000;
        
        [SerializeField] private ulong currentBetAmount;

        public ulong CurrentBetAmount
        {
            get
            {
                return currentBetAmount;
            }
            set
            {
                currentBetAmount = value;
                if (currentBetAmount < 1_000_000)
                    currentBetAmount = 1_000_000;
                onBetChange?.Invoke(value);
            }
        }

        public Action<ulong> onBetChange;
        
        //If it's not a free spin.
        public bool MakeBet()
        {
            if (CurrentBetAmount > playerData.Gold)
                return false;

            playerData.Gold -= CurrentBetAmount;
            return true;
        }

        public void ChangeBet(bool isIncrease)
        {
            if(isIncrease)
                CurrentBetAmount += betChangeAmount;
            else
            {
                CurrentBetAmount -= betChangeAmount;
            }
        }
    }
}