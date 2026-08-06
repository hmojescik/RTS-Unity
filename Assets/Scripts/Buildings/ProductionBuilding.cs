using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    [Header("Production")]
    public Transform spawnPoint;
    private Queue<GameObject> productionQueue = new Queue<GameObject>();
    private bool isProducing;

    protected IEnumerator Produce(GameObject prefab, float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
    public void AddToQueue(GameObject prefab)
    {
        productionQueue.Enqueue(prefab);

        if (!isProducing)
        {
            StartCoroutine(ProcessQueue());
        }
    }
    private IEnumerator ProcessQueue()
    {
        isProducing = true;

        while (productionQueue.Count > 0)
        {
            GameObject prefab = productionQueue.Dequeue();

            yield return new WaitForSeconds(3f);

            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }

        isProducing = false;
    }
}