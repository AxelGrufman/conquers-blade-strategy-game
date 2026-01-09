using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
   private NavMeshAgent agent;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false;
        //agent.updateRotation = false;     
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
    private void Update()
    {
       Vector3 SpawnLocation = transform.position - transform.forward * 0.5f + Vector3.up * 0.5f;
        Debug.DrawRay(SpawnLocation, transform.forward * 2f, Color.black);
    }
}


