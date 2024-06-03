using TMPro;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public PlayerData playerData;
    public TemporaryGoldPool temporaryGoldPool;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI freeSpinText;
    public TextMeshProUGUI nonCollectedGoldText;
    
    private void Start()
    {
        UpdateGoldText(playerData.Gold);
        UpdateFreeSpinText(playerData.FreeSpinAmount);
        UpdateNonCollectedGoldText(temporaryGoldPool.gold);
        playerData.onGoldChange += UpdateGoldText;
        playerData.onFreeSpinChange += UpdateFreeSpinText;
        temporaryGoldPool.onTemporaryGoldChange += UpdateNonCollectedGoldText;
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