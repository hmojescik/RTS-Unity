using System.Collections;
using UnityEngine;

public class Farm : Building
{
    [Header("Gold Production")]
    public int goldPerCycle = 5;
    public float productionInterval = 2f;

    private void Start()
    {
        StartCoroutine(ProduceGold());
    }

    private IEnumerator ProduceGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            ResourceManager.Instance.AddGold(goldPerCycle);
        }
    }
}