using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private UnitAttack attack;
    private UnitHealth myHealth;

    private void Start()
    {
        attack = GetComponentInParent<UnitAttack>();
        myHealth = GetComponentInParent<UnitHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger: " + other.name);

        UnitHealth enemy = other.GetComponentInParent<UnitHealth>();

        if (enemy == null)
            return;

        if (enemy.team == myHealth.team)
            return;

        if (attack.stance == UnitStance.Aggressive)
        {
            attack.SetTarget(enemy);
        }
    }
}