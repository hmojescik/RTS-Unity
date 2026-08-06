using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject barracksPrefab;
    public GameObject farmPrefab;
    public LayerMask groundLayer;
    private GameObject previewBuilding;
    private GameObject selectedBuilding;

    public bool IsBuilding => selectedBuilding != null;
    [Header("Building Costs")]
    public int barracksWoodCost = 150;
    public int barracksGoldCost = 50;

    public int farmWoodCost = 75;
    public int farmGoldCost = 25;
    public LayerMask blockingLayers;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectBuilding(GameObject prefab)
    {
        int woodCost = 0;
        int goldCost = 0;

        if (prefab == barracksPrefab)
        {
            woodCost = barracksWoodCost;
            goldCost = barracksGoldCost;
        }
        else if (prefab == farmPrefab)
        {
            woodCost = farmWoodCost;
            goldCost = farmGoldCost;
        }

        if (!ResourceManager.Instance.SpendWood(woodCost))
            return;

        if (!ResourceManager.Instance.SpendGold(goldCost))
        {
            ResourceManager.Instance.AddWood(woodCost);
            return;
        }

        selectedBuilding = prefab;

        if (previewBuilding != null)
            Destroy(previewBuilding);

        previewBuilding = Instantiate(prefab);

        // Wyłącz zaznaczenie
        Selectable selectable = previewBuilding.GetComponent<Selectable>();

        if (selectable != null && selectable.selectionMark != null)
        {
            selectable.selectionMark.SetActive(false);
        }

        // Wyłącz collider budynku
        Collider col = previewBuilding.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;

        // Wyłącz NavMeshObstacle
        UnityEngine.AI.NavMeshObstacle obstacle =
            previewBuilding.GetComponent<UnityEngine.AI.NavMeshObstacle>();

        if (obstacle != null)
            obstacle.enabled = false;
    }

    public void SelectBarracks()
    {
        SelectBuilding(barracksPrefab);
    }

    public void SelectFarm()
    {
        SelectBuilding(farmPrefab);
    }

    public void CancelBuilding()
    {
        selectedBuilding = null;
    }

    public GameObject GetSelectedBuilding()
    {
        return selectedBuilding;
    }

    private void Update()
    {
        if (!IsBuilding)
            return;

        UpdatePreview();

        if (!Input.GetMouseButtonDown(0))
            return;

        PlaceBuilding();
    }
    private void PlaceBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (((1 << hit.collider.gameObject.layer) & groundLayer) == 0)
            return;

        if (!CanPlaceBuilding(hit.point))
        {
            Debug.Log("Nie można postawić budynku.");
            return;
        }

        foreach (UnitMovement unit in UnitSelection.Instance.selectedUnits)
        {
            if (unit == null)
                continue;

            WorkerBuilder builder = unit.GetComponent<WorkerBuilder>();

            if (builder == null)
                continue;

            builder.Build(selectedBuilding, hit.point);
            break;
        }

        if (previewBuilding != null)
            Destroy(previewBuilding);

        CancelBuilding();
    }
    
    private void UpdatePreview()
    {
        Debug.Log("UpdatePreview");

        if (previewBuilding == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast nie trafił");
            return;
        }

        Debug.Log("Trafiono: " + hit.collider.name);

        if (((1 << hit.collider.gameObject.layer) & groundLayer) == 0)
        {
            Debug.Log("To nie Ground");
            return;
        }

        Debug.Log("Przesuwam preview");

        previewBuilding.transform.position = hit.point;

        bool canPlace = CanPlaceBuilding(hit.point);

        // Na razie tylko log
        Debug.Log(canPlace);
    }
    private bool CanPlaceBuilding(Vector3 position)
    {
        Vector3 halfExtents = new Vector3(2f, 1f, 2f);

        Collider[] hits = Physics.OverlapBox(
            position,
            halfExtents,
            Quaternion.identity,
            blockingLayers
        );

        return hits.Length == 0;
    }
}