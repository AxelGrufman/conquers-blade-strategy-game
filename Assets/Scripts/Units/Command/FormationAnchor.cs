using UnityEngine;
using UnityEngine.AI;

public class FormationAnchor : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    public bool HasReached()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return true;

        return false;
    }

    public Vector3 Position => transform.position;
    public Vector3 Forward => agent.velocity.sqrMagnitude > 0.01f
        ? agent.velocity.normalized
        : transform.forward;
}
