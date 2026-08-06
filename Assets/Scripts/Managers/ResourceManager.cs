using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int wood = 200;
    public int gold = 100;

    private void Awake()
    {
        Instance = this;
    }

    public bool SpendWood(int amount)
    {
        if (wood < amount)
            return false;

        wood -= amount;
        Debug.Log("Drewno: " + wood);
        return true;
    }

    public void AddWood(int amount)
    {
        wood += amount;
        Debug.Log("Drewno: " + wood);
    }

    public bool SpendGold(int amount)
    {
        if (gold < amount)
            return false;

        gold -= amount;
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
