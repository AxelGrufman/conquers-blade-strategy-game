using UnityEngine;

public enum EnemiState
{
    MovingToFlag,
    MovingToEnemies,
    Attacking
}

public class EnemiAi : MonoBehaviour
{
    public EnemiState State;

    public EnemioDetector detector;
    public Movement movement;
    public Transform Flag;
    public Transform ClosesTarrget;

    public float AttacRange = 2f;
    public int AttackDamage = 10;
    public float AttackCooldown = 0.75f;

    private float nextAttackTime;
    private Health cachedTargetHealth;
    private Transform cachedTargetTransform;

    private void Awake()
    {
        detector = GetComponent<EnemioDetector>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        ClosesTarrget = FindClosestInRange();
        CacheTargetComponentsIfChanged(ClosesTarrget);
        UpdateState();

        switch (State)
        {
            case EnemiState.MovingToFlag:
                MoveToFlag();
                break;

            case EnemiState.MovingToEnemies:
                MoveToClosestEnemy();
                break;

            case EnemiState.Attacking:
                AttackTarget();
                break;
        }
    }

    private void UpdateState()
    {
        if (ClosesTarrget == null)
        {
            State = EnemiState.MovingToFlag;
            return;
        }

        State = IsInAttackRange(ClosesTarrget)
            ? EnemiState.Attacking
            : EnemiState.MovingToEnemies;
    }

    private Transform FindClosestInRange()
    {
        if (detector == null || detector.InRange == null || detector.InRange.Count == 0)
            return null;

        Transform closest = null;
        float bestSqr = Mathf.Infinity;
        Vector3 pos = transform.position;

        for (int i = detector.InRange.Count - 1; i >= 0; i--)
        {
            Transform t = detector.InRange[i];

            if (t == null)
            {
                detector.InRange.RemoveAt(i);
                continue;
            }

            float sqr = (t.position - pos).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                closest = t;
            }
        }

        return closest;
    }

    private void CacheTargetComponentsIfChanged(Transform newTarget)
    {
        if (newTarget == cachedTargetTransform)
            return;

        cachedTargetTransform = newTarget;
        cachedTargetHealth = newTarget ? newTarget.GetComponentInParent<Health>() : null;
    }

    private bool IsInAttackRange(Transform target)
    {
        return (target.position - transform.position).sqrMagnitude <= AttacRange * AttacRange;
    }

    private void AttackTarget()
    {
        movement.Stop();

        if (!cachedTargetTransform || cachedTargetHealth == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + AttackCooldown;
        cachedTargetHealth.TakeDamage(AttackDamage);
    }

    private void MoveToFlag()
    {
        if (!Flag) return;
       movement.MoveTo(Flag.position);
    }

    private void MoveToClosestEnemy()
    {
        if (!ClosesTarrget) return;
        movement.MoveTo(ClosesTarrget.position);
    }
}
