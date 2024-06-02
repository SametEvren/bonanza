using UnityEngine;

public class BetController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public ulong CurrentBetAmount
    {
        get;
        private set;
    }

    public void SetBetAmount(ulong betAmount)
    {
        CurrentBetAmount = betAmount;
    }

    //If it's not a free spin.
    public bool MakeBet()
    {
        if (CurrentBetAmount > playerData.Gold)
            return false;

        playerData.Gold -= CurrentBetAmount;
        return true;
    }
}