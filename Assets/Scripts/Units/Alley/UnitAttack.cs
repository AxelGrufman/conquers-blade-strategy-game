using UnityEngine;

public class UnitAttack : MonoBehaviour
{
   public float AttackRange = 2f;
    public int AttackDamage = 10;
    public float AttackCooldown = 0.75f;
    private float nextAttackTime;
    private Health targetHealth;
    public GameObject Target;
    private void Update()
    {
        if (Target != null)
        {
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            if (distance <= AttackRange && Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
    }
    public void SetTarget(GameObject target)
    {
        Target = target;
        targetHealth = target.GetComponent<Health>();
    }
    private void Attack()
    {
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(AttackDamage);
            nextAttackTime = Time.time + AttackCooldown;
        }
    }
}
