using UnityEngine;

public class UnitMovement : Unit
{
    Camera cam;
    UnityEngine.AI.NavMeshAgent agent;

    public LayerMask groundLayer;

    private UnitMovement movement;

    private void Awake()
    {
        movement = GetComponent<UnitMovement>();
    }

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        
    }
    private void OnEnable()
    {
        UnitSelection.allUnits.Add(this);
    }

    private void OnDisable()
    {
        UnitSelection.allUnits.Remove(this);
        UnitSelection.Instance.selectedUnits.Remove(this);
    }

    public void MoveTo(Vector3 position)
    {
        Debug.Log("MoveTo wywołane");
        agent.SetDestination(position);
    }
}