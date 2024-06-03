using GameplayControllers;
using TMPro;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    [SerializeField] private BetController betController;
    [SerializeField] private SlotMachineController slotMachineController;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TemporaryGoldPool temporaryGoldPool;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI freeSpinText;
    [SerializeField] private TextMeshProUGUI nonCollectedGoldText;
    [SerializeField] private TextMeshProUGUI betAmountText;
    [SerializeField] private TextMeshProUGUI autoSpinButtonText;
    [SerializeField] private GameObject spinButton;

    private void Start()
    {
        UpdateGoldText(playerData.Gold);
        UpdateFreeSpinText(playerData.FreeSpinAmount);
        UpdateNonCollectedGoldText(temporaryGoldPool.Gold);
        UpdateBetAmountText(betController.CurrentBetAmount);

        playerData.onGoldChange += UpdateGoldText;
        playerData.onFreeSpinChange += UpdateFreeSpinText;
        temporaryGoldPool.onTemporaryGoldChange += UpdateNonCollectedGoldText;
        betController.onBetChange += UpdateBetAmountText;
        slotMachineController.AutoSpinChanged += ChangeSpinButtons;
    }

    private void OnDestroy()
    {
        playerData.onGoldChange -= UpdateGoldText;
        playerData.onFreeSpinChange -= UpdateFreeSpinText;
        temporaryGoldPool.onTemporaryGoldChange -= UpdateNonCollectedGoldText;
        betController.onBetChange -= UpdateBetAmountText;
        slotMachineController.AutoSpinChanged -= ChangeSpinButtons;
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

    private void ChangeSpinButtons(bool isAutoSpinEnabled)
    {
        if (isAutoSpinEnabled)
        {
            autoSpinButtonText.text = "Auto Spin Enabled";
            spinButton.SetActive(false);
        }
        else
        {
            autoSpinButtonText.text = "Auto Spin Disabled";
            spinButton.SetActive(true);
        }
    }
}
