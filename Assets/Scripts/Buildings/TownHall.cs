using UnityEngine;

public class TownHall : ProductionBuilding
{
    public GameObject workerPrefab;
    public int workerWoodCost = 50;

    public void TrainWorker()
    {
        if (!ResourceManager.Instance.SpendWood(workerWoodCost))
            return;

        AddToQueue(workerPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrainWorker();
        }
    }
}