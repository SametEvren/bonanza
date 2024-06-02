using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    public void ChangeGold(ulong value)
    {
        playerData.Gold += value;
    }
}