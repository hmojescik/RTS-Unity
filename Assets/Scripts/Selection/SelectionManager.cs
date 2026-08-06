using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : MonoBehaviour
{
    public LayerMask groundLayer;
    public static UnitSelection Instance;
    public Selectable selectedObject;

    public UnitMovement selectedUnit;
    public List<UnitMovement> selectedUnits = new List<UnitMovement>();
    public static List<UnitMovement> allUnits = new List<UnitMovement>();
    private Vector2 startPosition;
    private Vector2 endPosition;

    private bool isDragging;

    Camera cam;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Selectable selectable = hit.collider.GetComponentInParent<Selectable>();

                if (selectable != null)
                {
                    if (selectedObject != null)
                        selectedObject.Deselect();

                    selectedObject = selectable;
                    selectedObject.Select();
                }
                
                UnitHealth enemy = hit.collider.GetComponent<UnitHealth>();

                if (enemy != null)
                {
                    foreach (UnitMovement unit in selectedUnits)
                    {
                        if (unit == null)
                        continue;

                        if (enemy.team != unit.GetComponent<UnitHealth>().team)
                        {
                            unit.GetComponent<UnitAttack>().SetTarget(enemy);
                        }                    
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // wykonaj zaznaczanie ramką tylko przy rzeczywistym przeciągnięciu
            if (Vector2.Distance(startPosition, endPosition) > 10f)
            {
                SelectUnitsInBox();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void OnGUI()
    {
        if(!isDragging)
            return;
        Rect rect = GetScreenRect(startPosition, endPosition);

        GUI.color = new Color(0, 1, 0, 0.2f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);

        GUI.color = Color.green;

        DrawBorder(rect, 2);
    }
    Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        start.y = Screen.height - start.y;
        end.y = Screen.height - end.y;

        Vector2 topLeft = Vector2.Min(start, end);
        Vector2 bottomRight = Vector2.Max(start, end);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
    void DrawBorder(Rect rect, float thickness)
    {
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);
    }
    void SelectUnitsInBox()
    {
        // Odznacz poprzednio zaznaczony obiekt
        foreach (UnitMovement unit in selectedUnits)
        {
            if (unit != null)
            {
                Selectable selectable = unit.GetComponent<Selectable>();

                if (selectable != null)
                    selectable.Deselect();
            }
        }

        selectedUnits.Clear();
        selectedObject = null;

        selectedUnits.Clear();

        Rect rect = GetScreenRect(startPosition, endPosition);

        foreach (UnitMovement unit in allUnits)
        {
            if (unit == null)
                continue;

            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            screenPos.y = Screen.height - screenPos.y;

            if (rect.Contains(screenPos))
            {
                selectedUnits.Add(unit);

                Selectable selectable = unit.GetComponent<Selectable>();

                if (selectable != null)
                {
                    selectable.Select();

                    // zapamiętaj pierwszy zaznaczony obiekt
                    if (selectedObject == null)
                        selectedObject = selectable;
                }
            }
        }
    }

    void HandleRightClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;
        
        Debug.Log("Kliknięto: " + hit.collider.name);
        
        Tree tree = hit.collider.GetComponentInParent<Tree>();

        if (tree != null)
        {
            Debug.Log("Wykryto drzewo");

            Debug.Log(selectedUnits.Count);   // <-- TUTAJ

            foreach (UnitMovement unit in selectedUnits)
            {
                if (unit == null)
                    continue;

                Debug.Log("Jednostka: " + unit.name);

                WorkerGather worker = unit.GetComponent<WorkerGather>();

                if (worker == null)
                {
                    Debug.Log("BRAK WorkerGather!");
                    continue;
                }

                Debug.Log("Znaleziono WorkerGather");

                worker.Gather(tree);
            }

            return;
        }

        UnitHealth enemy = hit.collider.GetComponentInParent<UnitHealth>();

        if (enemy != null)
        {
            foreach (UnitMovement unit in selectedUnits)
            {
                if (unit == null)
                continue;

                UnitHealth myHealth = unit.GetComponent<UnitHealth>();

                if (enemy.team != myHealth.team)
                {
                    unit.GetComponent<UnitAttack>().SetTarget(enemy);
                }
            }
            return;
        }

        if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {
            foreach (UnitMovement unit in selectedUnits)
            {
                if (unit == null)
                continue;

                unit.GetComponent<UnitAttack>().ClearTarget();
                unit.MoveTo(hit.point);
            }
        }
    }
}