using UnityEngine;

public class Barracks : ProductionBuilding
{
    public GameObject soldierPrefab;
    public int soldierWoodCost = 50;
    public int soldierGoldCost = 25;

    public void TrainSoldier()
    {
        if (!ResourceManager.Instance.SpendWood(soldierWoodCost))
            return;

        if (!ResourceManager.Instance.SpendGold(soldierGoldCost))
        {
            ResourceManager.Instance.AddWood(soldierWoodCost);
            return;
        }

        AddToQueue(soldierPrefab);
    }
}
