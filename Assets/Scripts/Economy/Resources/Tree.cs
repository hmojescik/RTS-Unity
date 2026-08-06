using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int maxWoodAmount = 150;
    public float respawnTime = 30f;

    private int woodAmount;

    private Collider treeCollider;
    private Renderer[] renderers;

    private void Awake()
    {
        woodAmount = maxWoodAmount;

        treeCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public bool HasWood()
    {
        return woodAmount > 0;
    }

    public int Harvest(int amount)
    {
        int harvested = Mathf.Min(amount, woodAmount);

        woodAmount -= harvested;

        Debug.Log("Pozostało drewna: " + woodAmount);

        if (woodAmount <= 0)
        {
            Debug.Log("Drzewo ścięte");
            StartCoroutine(RespawnTree());
        }

        return harvested;
    }

    private IEnumerator RespawnTree()
    {
        // Wyłącz drzewo
        treeCollider.enabled = false;

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        // Odródź drzewo
        woodAmount = maxWoodAmount;

        treeCollider.enabled = true;

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }
}