using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Movment : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    NavMeshAgent agent;
    [SerializeField] private int Id;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = 0f;        // IMPORTANT
        agent.autoBraking = false;          // STOPS WEIRD DECELERATION
        agent.updateRotation = false;       // We control rotation manually
    }



    public void MoveTo(Vector3 targetPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public Vector3 GetVelocity()
    {
        return agent.velocity;
    }



}


