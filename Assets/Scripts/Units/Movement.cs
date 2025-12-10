using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    NavMeshAgent agent;
    [SerializeField] private int Id;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false;          
        agent.updateRotation = false;     
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


