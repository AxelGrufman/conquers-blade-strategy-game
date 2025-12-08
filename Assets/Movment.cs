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
    }


    public bool MoveToTarget(Vector3 targetPosition)
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(targetPosition);
            return true;
        }
        return false;
    }
}


