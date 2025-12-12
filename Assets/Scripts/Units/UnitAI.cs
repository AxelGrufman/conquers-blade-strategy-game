using UnityEngine;

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

    [Header("Combat Settings")]
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float maxChaseDistance = 15f;

    [Header("Guard Settings")]
    private Vector3 guardCenter;
    private float guardRadius;

    private Movement movement;
    private GameObject currentTarget;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
    
        switch (State)
        {
            case UnitState.Holding:
            case UnitState.Guarding:
            case UnitState.AttackAtWill:
            case UnitState.Attacking:
                HandleCombatLogic();
                break;

            case UnitState.ReturningToFormation:
                HandleReturnToFormation();
                break;
        }


        switch (State)
        {
            case UnitState.Following:
            case UnitState.Moving:
            case UnitState.Holding:
            case UnitState.Guarding:
            case UnitState.AttackAtWill:
            case UnitState.Attacking:
                AlignRotationToMovement();
                break;
            case UnitState.ReturningToFormation:
                AlignRotationToMovement();
                break;
        }
    }

    public void SetState(UnitState newState)
    {
        State = newState;
    }

    public void GoTo(Vector3 pos)
    {
        SetState(UnitState.Moving);
        movement.MoveTo(pos);
    }

    public void HoldAtPosition(Vector3 pos)
    {
        State = UnitState.Holding;
        movement.MoveTo(pos);
    }

    public void FollowFormationSlot(Vector3 slot)
    {
        State = UnitState.Following;
        movement.MoveTo(slot);
    }

    public void ReturnToFormation(Vector3 pos)
    {
        State = UnitState.ReturningToFormation;
        movement.MoveTo(pos);
    }

    public void Attack(GameObject target)
    {
        currentTarget = target;
        SetState(UnitState.Attacking);
    }


    public void EnableAttackAtWill()
    {
        State = UnitState.AttackAtWill;
  
        guardCenter = transform.position;
        guardRadius = maxChaseDistance;
    }


    public void SetGuardArea(Vector3 center, float radius)
    {
        guardCenter = center;
        guardRadius = radius;
        State = UnitState.Guarding;


        movement.MoveTo(guardCenter);
    }

    private void HandleCombatLogic()
    {
        if (currentTarget == null || !IsEnemyValid(currentTarget))
        {
            currentTarget = FindClosestEnemy();

            if (currentTarget == null)
            {
                if (State == UnitState.Guarding)
                    ReturnTowardsGuardCenter();

                return;
            }
        }

        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (dist <= attackRange)
        {
            movement.Stop();
            FaceTarget(currentTarget.transform.position);
        }
        else
        {
            if (CanChaseTarget(currentTarget.transform.position))
            {
                movement.MoveTo(currentTarget.transform.position);
            }
            else
            {
 
                currentTarget = null;

                if (State == UnitState.Guarding)
                    ReturnTowardsGuardCenter();
            }
        }
    }

    private void HandleReturnToFormation()
    {
        if (movement.GetVelocity().sqrMagnitude < 0.01f)
        {
            State = UnitState.Holding; 
        }
    }

    private GameObject FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
        float bestDist = Mathf.Infinity;
        GameObject best = null;

        foreach (var h in hits)
        {
            float d = Vector3.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = h.gameObject;
            }
        }

        return best;
    }

    private bool IsEnemyValid(GameObject enemy)
    {
        if (!enemy) return false;
        return true;
    }

    private bool CanChaseTarget(Vector3 targetPos)
    {
        if (State == UnitState.AttackAtWill)
            return Vector3.Distance(transform.position, guardCenter) <= maxChaseDistance * 2f;

        if (State == UnitState.Guarding)
            return Vector3.Distance(guardCenter, targetPos) <= guardRadius + maxChaseDistance;

        return true;
    }

    private void ReturnTowardsGuardCenter()
    {
        float dist = Vector3.Distance(transform.position, guardCenter);
        if (dist > 1f)
            movement.MoveTo(guardCenter);
        else
            movement.Stop();
    }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 7f);
        }
    }

    private void AlignRotationToMovement()
    {
        Vector3 vel = movement.GetVelocity();
        vel.y = 0;

        if (vel.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(vel);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 7);
        }
    }
}
