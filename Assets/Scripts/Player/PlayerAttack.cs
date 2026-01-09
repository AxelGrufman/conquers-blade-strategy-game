using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRadius = 5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 0.75f;

    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayers;

    // Non-alloc buffer to avoid GC spikes
    [SerializeField] private int maxEnemiesToCheck = 32;
    private Collider[] hits;

    private float nextAttackTime;

    private void Awake()
    {
        hits = new Collider[Mathf.Max(1, maxEnemiesToCheck)];
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        Debug.Log("Player Attack Input Received");

        if (Time.time < nextAttackTime) return;
        Debug.Log("Player Attacking");

        Health target = FindClosestEnemyHealth();
        if (target == null) return;
        Debug.Log("Player Found Target to Attack: " + target.name);

        target.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackCooldown;
    }

    private Health FindClosestEnemyHealth()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            attackRadius,
            hits,
            enemyLayers,
            QueryTriggerInteraction.Collide
        );

        if (count == 0) return null;

        Health closestHealth = null;
        float bestDistSq = float.PositiveInfinity;
        Vector3 origin = transform.position;

        for (int i = 0; i < count; i++)
        {
            Collider c = hits[i];
            if (c == null) continue;

            // No children involved per your setup, so plain GetComponent is fine.
            Health h = c.GetComponent<Health>();
            if (h == null) continue;

            float distSq = (c.transform.position - origin).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                closestHealth = h;
            }
        }

        return closestHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
