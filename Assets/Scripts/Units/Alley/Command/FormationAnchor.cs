using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class FormationAnchor : MonoBehaviour
{
    public NavMeshAgent agent;
    public OrderGiver OrderGiver;
    public float RotationSpeed = 1f;
    public Quaternion Rotaiton;

    private void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();

        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.avoidancePriority = 0; 
    }

    public void MoveTo(Vector3 target, Vector3[] Slots)
    {
        agent.isStopped = false;
        agent.SetDestination(target);

        for (int i = 0; i < OrderGiver.Units.Count; i++)
        {
            OrderGiver.Units[i].GetComponent<UnitAI>().GoTo(Slots[i]);
        }
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
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

    private void Update()
    {
        Vector3 desiredForward =
    agent.velocity.sqrMagnitude > 0.01f
        ? agent.velocity.normalized
        : transform.forward;

        if (desiredForward.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(desiredForward, Vector3.up);

        float t = 1f - Mathf.Exp(-RotationSpeed * Time.deltaTime);
        Rotaiton = Quaternion.Slerp(Rotaiton, targetRotation, t);

        Debug.DrawRay(transform.position, desiredForward * 2f, Color.white);
        Debug.DrawRay(transform.position, Rotaiton * Vector3.forward * 2f, Color.cyan);
    }


}
