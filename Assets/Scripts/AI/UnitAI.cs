using UnityEngine;

public class UnitAI : MonoBehaviour
{
    public float searchRange = 1000f;
    public float searchInterval = 0.5f;

    private UnitAttack attack;
    private UnitHealth health;

    private float nextSearch;

    private void Awake()
    {
        attack = GetComponent<UnitAttack>();
        health = GetComponent<UnitHealth>();
    }

    private void Update()
    {
        if (Time.time < nextSearch)
            return;

        nextSearch = Time.time + searchInterval;

        if (attack == null)
            return;

        // Jeżeli już walczy, nic nie rób
        if (attack.HasTarget)
            return;

        FindTarget();
    }

    private void FindTarget()
    {
        UnitHealth[] units = FindObjectsByType<UnitHealth>(FindObjectsSortMode.None);

        UnitHealth closestSoldier = null;
        UnitHealth closestWorker = null;
        UnitHealth closestBuilding = null;
        UnitHealth closestTownHall = null;

        float soldierDistance = searchRange;
        float workerDistance = searchRange;
        float buildingDistance = searchRange;
        float townHallDistance = searchRange;

        foreach (UnitHealth unit in units)
        {
            if (unit == health)
                continue;

            if (unit.team == health.team)
                continue;

            float distance = Vector3.Distance(transform.position, unit.transform.position);

            // TownHall
            if (unit.GetComponent<TownHall>() != null)
            {
                if (distance < townHallDistance)
                {
                    townHallDistance = distance;
                    closestTownHall = unit;
                }

                continue;
            }

            // Budynek
            if (unit.GetComponent<Building>() != null)
            {
                if (distance < buildingDistance)
                {
                    buildingDistance = distance;
                    closestBuilding = unit;
                }

                continue;
            }

            // Żołnierz
            if (unit.GetComponent<UnitAttack>() != null)
            {
                if (distance < soldierDistance)
                {
                    soldierDistance = distance;
                    closestSoldier = unit;
                }

                continue;
            }

            // Worker
            if (unit.GetComponent<WorkerGather>() != null)
            {
                if (distance < workerDistance)
                {
                    workerDistance = distance;
                    closestWorker = unit;
                }
            }
        }

        if (closestSoldier != null)
        {
            attack.SetTarget(closestSoldier);
        }
        else if (closestWorker != null)
        {
            attack.SetTarget(closestWorker);
        }
        else if (closestBuilding != null)
        {
            attack.SetTarget(closestBuilding);
        }
        else if (closestTownHall != null)
        {
            attack.SetTarget(closestTownHall);
        }
    }
}