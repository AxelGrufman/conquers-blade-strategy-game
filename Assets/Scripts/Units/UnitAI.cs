using UnityEngine;

public enum UnitState
{
    Idle,
    Moving,
    Following,
    Holding,
    Attacking,
    ReturningToFormation
}

public class UnitAI : MonoBehaviour
{
    public UnitState State;
    private Movment movement;

    private void Awake()
    {
        movement = GetComponent<Movment>();
    }

    private void Update()
    {
        if (State == UnitState.Holding)
        {
            GameObject enemy = EnemyInRange();

            if (enemy != null)
                Attack(enemy);
        }

        switch (State)
        {
            case UnitState.Following:
            case UnitState.Moving:
            case UnitState.Holding:
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


    public void Attack(GameObject target)
    {
        SetState(UnitState.Attacking);
    }
    private GameObject EnemyInRange()
    {
        // Placeholder for enemy detection logic
        return null;
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
