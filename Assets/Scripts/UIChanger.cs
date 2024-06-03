using TMPro;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public PlayerData playerData;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI freeSpinText;

    private void Start()
    {
        UpdateGoldText(playerData.Gold);
        UpdateFreeSpinText(playerData.FreeSpinAmount);
        playerData.onGoldChange += UpdateGoldText;
        playerData.onFreeSpinChange += UpdateFreeSpinText;
    }

    private void UpdateFreeSpinText(int newFreeSpin)
    {
        freeSpinText.text = newFreeSpin.ToString();
    }

    private void UpdateGoldText(ulong newGold)
    {
        goldText.text = newGold.ToString();
    }
}