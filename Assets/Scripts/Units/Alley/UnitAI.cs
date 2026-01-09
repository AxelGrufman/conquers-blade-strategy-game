using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    Moving,
    Following,
    Holding,
    Attacking,
    ReturningToFormation,
    Guarding,
    AttackAtWill
}

public class UnitAI : MonoBehaviour
{
    public UnitState State;

    private Movement movement;
    private GameObject currentTarget;

    public float detectRange;
    public LayerMask enemyMask;
    private UnitAttack UnitAttack;



    private void Awake()
    {
        movement = GetComponent<Movement>();
        UnitAttack = GetComponent<UnitAttack>();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = 50;
    }

    //public void SetState(UnitState newState)
    //{
    //    State = newState;
    //}

    public void GoTo(Vector3 pos)
    {
        //SetState(UnitState.Moving);
        movement.MoveTo(pos);
    }

    public void HoldAtPosition(Vector3 pos)
    {
        //State = UnitState.Holding;
        movement.MoveTo(pos);
    }

    public void FollowFormationSlot(Vector3 slot)
    {
        //State = UnitState.Following;
        movement.MoveTo(slot);
    }

    private void Update()
    {
        GameObject closest = GetClosestEnemy(transform.position, detectRange, enemyMask);
        if(closest != null)
        {
            Debug.Log("Closest enemy found: " + closest.name);
            UnitAttack.SetTarget(closest);
        }
    }


    public static GameObject GetClosestEnemy(Vector3 origin, float range, LayerMask enemyMask)
    {
        Collider[] hits = Physics.OverlapSphere(origin, range, enemyMask, QueryTriggerInteraction.Collide);

        GameObject closest = null;
        float bestSqr = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject t = hits[i].gameObject;

            float sqr = (t.transform.position - origin).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                closest = t;
            }
        }

        return closest;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }


}
