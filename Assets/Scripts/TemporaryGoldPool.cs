using UnityEngine;

[CreateAssetMenu(fileName = "Temporary Gold Pool")]

public class TemporaryGoldPool : ScriptableObject
{
    public PlayerData playerData;
    public ulong gold;
    public float multiplier;

    public void ApplyToPlayer()
    {
        Debug.Log("Added Gold is: " + gold + " Multiplier is: " + multiplier);
        playerData.Gold += (ulong)(gold * Mathf.Max(multiplier,1));
        Debug.Log((ulong)(gold * Mathf.Max(multiplier,1)));
        gold = 0;
        multiplier = 0;
    }
}
