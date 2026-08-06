using UnityEngine;
using UnityEngine.AI;

public enum WorkerState
{
    Idle,
    MovingToTree,
    Gathering,
    ReturningToTownHall
}
public class WorkerGather : MonoBehaviour
{
    [Header("Gathering")]
    public float gatherRange = 2f;
    public float gatherInterval = 1f;
    public int woodPerHit = 10;
    public int carryCapacity = 50;
    private bool returningStarted;

    private WorkerState state = WorkerState.Idle;

    private Tree targetTree;
    private Tree currentTree;
    private TownHall homeTownHall;

    private UnitMovement movement;
    private NavMeshAgent agent;
    private UnitHealth health;

    private int carriedWood;
    private float nextGatherTime;

    public int CarriedWood => carriedWood;
    public bool IsCarryingWood => carriedWood > 0;

    private void Awake()
    {
        movement = GetComponent<UnitMovement>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<UnitHealth>();
    }

    private void Start()
    {
        TownHall[] townHalls = FindObjectsByType<TownHall>(FindObjectsSortMode.None);

        foreach (TownHall hall in townHalls)
        {
            UnitHealth hallHealth = hall.GetComponent<UnitHealth>();

            if (hallHealth != null && hallHealth.team == health.team)
            {
                homeTownHall = hall;
                break;
            }
        }
    }

    public void Gather(Tree tree)
    {
        Debug.Log("gath");
        Debug.Log("ścinanie polecenie");
        if (tree == null)
            return;

        currentTree = tree;
        targetTree = tree;

        state = WorkerState.MovingToTree;

        movement.MoveTo(tree.transform.position);
    }

    private void Update()
    {
        Debug.Log("State: " + state);
        switch (state)
        {
            case WorkerState.Idle:
                break;

            case WorkerState.MovingToTree:
            case WorkerState.Gathering:
                UpdateGathering();
                break;

            case WorkerState.ReturningToTownHall:
                UpdateReturning();
                break;
        }
    }

    private void UpdateGathering()
    {
        if (targetTree == null)
        {
            StopGathering();
            return;
        }

        float distance = Vector3.Distance(transform.position, targetTree.transform.position);

        Debug.Log($"Distance: {distance}, GatherRange: {gatherRange}");
        if (distance > gatherRange)
        {
            movement.MoveTo(targetTree.transform.position);
            return;
        }

        state = WorkerState.Gathering;

        agent.ResetPath();

        FaceTree();

        if (Time.time < nextGatherTime)
            return;

        nextGatherTime = Time.time + gatherInterval;
        Debug.Log("Następne zbieranie o: " + nextGatherTime);

        GatherWood();
    }

    private void UpdateReturning()
    {
        if (homeTownHall == null)
            return;

        if (!returningStarted)
        {
            movement.MoveTo(homeTownHall.transform.position);
            returningStarted = true;
        }

        // Czekaj, aż agent faktycznie dojdzie do celu
        if (agent.pathPending)
            return;

        if (agent.remainingDistance > agent.stoppingDistance)
            return;

        returningStarted = false;

        DepositWood();

        Tree nextTree = FindClosestTree();

        if (nextTree != null)
        {
            Gather(nextTree);
        }
        else
        {
            StopGathering();
        }
    }

    private void GatherWood()
    {
        Debug.Log("GatherWood START");

        if (targetTree == null)
        {
            Debug.Log("targetTree == null");
            return;
        }

        Debug.Log("Przed Harvest. Drewno w drzewie: " + targetTree.HasWood());

        int wood = targetTree.Harvest(woodPerHit);

        Debug.Log("Harvest zwrócił: " + wood);

        carriedWood += wood;

        Debug.Log("Worker niesie: " + carriedWood);

        if (carriedWood >= carryCapacity)
        {
            targetTree = null;
            state = WorkerState.ReturningToTownHall;
            return;
        }

        if (!targetTree.HasWood())
        {
            StopGathering();
        }
    }

    private void DepositWood()
    {
        ResourceManager.Instance.AddWood(carriedWood);
        carriedWood = 0;
    }

    private void StopGathering()
    {
        targetTree = null;
        currentTree = null;
        state = WorkerState.Idle;
    }

    private void FaceTree()
    {
        if (targetTree == null)
            return;

        Vector3 direction = targetTree.transform.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8f);
    }
    private Tree FindClosestTree()
    {
        Tree[] trees = FindObjectsByType<Tree>(FindObjectsSortMode.None);

        Tree closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Tree tree in trees)
        {
            if (!tree.HasWood())
                continue;

            float distance = Vector3.Distance(transform.position, tree.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = tree;
            }
        }

        return closest;
    }
}