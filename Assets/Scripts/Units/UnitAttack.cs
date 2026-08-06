using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int damage = 20;
    public float attackCooldown = 1f;
    public float detectionRange = 1000f;
    public UnitStance stance = UnitStance.Aggressive;
    private float nextAttackTime;
    private bool hasAttackOrder = false;

    private UnitHealth target;
    public bool HasTarget => target != null;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void SetTarget(UnitHealth enemy)
    {
        if (enemy == null)
            return;

        target = enemy;
        hasAttackOrder = true;
    }

    private void Update()
    {
        if (target == null)
        {
            ClearTarget();
            return;
        }
        if (!hasAttackOrder || target == null)
        return;

        HandleMovement();
        FaceTarget();
        HandleAttack();
    }
    private void HandleMovement()
    {
        Collider targetCollider = target.GetComponent<Collider>();

        Vector3 closestPoint = targetCollider != null? targetCollider.ClosestPoint(transform.position): target.transform.position;

        float distance = Vector3.Distance(transform.position, closestPoint);

        if (distance > attackRange)
        {
            agent.SetDestination(GetAttackPosition());
        }
        else
        {
            agent.ResetPath();
        }
    }

    private void HandleAttack()
    {
        Collider targetCollider = target.GetComponent<Collider>();

        Vector3 closestPoint = targetCollider != null? targetCollider.ClosestPoint(transform.position): target.transform.position;

        float distance = Vector3.Distance(transform.position, closestPoint);

        if (distance > attackRange)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        target.TakeDamage(damage);
    }

    public void ClearTarget()
    {
        target = null;
        hasAttackOrder = false;
    }
    private Vector3 GetAttackPosition()
    {
        Vector3 direction = (transform.position - target.transform.position).normalized;

        if (direction == Vector3.zero)
            direction = Random.onUnitSphere;

        direction.y = 0;

        return target.transform.position + direction * (attackRange - 0.2f);
    }
    private void FaceTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }
}