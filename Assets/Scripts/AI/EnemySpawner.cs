using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemySoldierPrefab;
    public Transform spawnPoint;

    public float spawnInterval = 20f;
    public int soldiersPerWave = 4;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (!GameManager.Instance.gameEnded)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < soldiersPerWave; i++)
            {
                Instantiate(enemySoldierPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}