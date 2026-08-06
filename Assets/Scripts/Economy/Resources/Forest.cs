using System.Collections;
using UnityEngine;

public class Forest : MonoBehaviour
{
    public GameObject treePrefab;
    public float respawnTime = 30f;

    public void RespawnTree(Vector3 position)
    {
        StartCoroutine(Respawn(position));
    }

    private IEnumerator Respawn(Vector3 position)
    {
        yield return new WaitForSeconds(respawnTime);

        Instantiate(treePrefab, position, Quaternion.identity, transform);
    }
}