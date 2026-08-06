using UnityEngine;

public class WorkerBuilder : MonoBehaviour
{
    private UnitMovement movement;

    private GameObject buildingPrefab;
    private Vector3 buildPosition;

    private bool isBuilding;

    private void Awake()
    {
        movement = GetComponent<UnitMovement>();
    }

    public void Build(GameObject prefab, Vector3 position)
    {
        Debug.Log("Worker dostał do budowy: " + prefab.name);
        buildingPrefab = prefab;
        buildPosition = position;

        Debug.Log("Pozycja budowy: " + buildPosition);

        isBuilding = true;

        movement.MoveTo(buildPosition);
    }

    private void Update()
    {
        if (!isBuilding)
            return;

        float distance = Vector3.Distance(transform.position, buildPosition);
        Debug.Log("Distance: " + distance);

        if (distance > 2f)
            return;

        Debug.Log("Budowa zakończona");
        Debug.Log("Buduję w: " + buildPosition);

        Instantiate(buildingPrefab, buildPosition + Vector3.up * 1f, Quaternion.identity);

        buildingPrefab = null;
        isBuilding = false;
    }
}