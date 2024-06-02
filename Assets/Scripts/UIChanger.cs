using TMPro;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public PlayerData playerData;

    private void Start()
    {
        playerData.onGoldChange += UpdateGoldText;
    }

    private void UpdateGoldText(ulong newGold)
    {
        goldText.text = newGold.ToString();
    }
}