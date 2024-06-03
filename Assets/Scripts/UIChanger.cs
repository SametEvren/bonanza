using GameplayControllers;
using TMPro;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    [SerializeField] private BetController betController;
    public PlayerData playerData;
    public TemporaryGoldPool temporaryGoldPool;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI freeSpinText;
    public TextMeshProUGUI nonCollectedGoldText;
    public TextMeshProUGUI betAmountText;
    
    private void Start()
    {
        UpdateGoldText(playerData.Gold);
        UpdateFreeSpinText(playerData.FreeSpinAmount);
        UpdateNonCollectedGoldText(temporaryGoldPool.gold);
        UpdateBetAmountText(betController.CurrentBetAmount);
        playerData.onGoldChange += UpdateGoldText;
        playerData.onFreeSpinChange += UpdateFreeSpinText;
        temporaryGoldPool.onTemporaryGoldChange += UpdateNonCollectedGoldText;
        betController.onBetChange += UpdateBetAmountText;
    }

    private void UpdateBetAmountText(ulong betAmount)
    {
        betAmountText.text = betAmount.ToString();
    }

    private void UpdateFreeSpinText(int newFreeSpin)
    {
        freeSpinText.text = newFreeSpin.ToString();
    }

    private void UpdateGoldText(ulong newGold)
    {
        goldText.text = newGold.ToString();
    }

    private void UpdateNonCollectedGoldText(ulong newGold)
    {
        nonCollectedGoldText.text = newGold.ToString();
    }
}