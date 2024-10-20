using UnityEngine;
using UnityEngine.AI;

public class Unit : Proc
{
    public float speed = 5f;

    
    protected int status = 0;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
    }


    protected void MoveToPosition(Vector3Int cell)
    {
        agent.destination = MoveGrid.CellPosition(cell);
        agent.isStopped = false;
    }
    protected void MoveToPosition(Vector3 pos)
    {
        agent.destination = MoveGrid.CellPosition(pos);
        agent.isStopped = false;
    }

    protected virtual void Update()
    {
        if (status == 1)
        {
            if (HasDestination())
            {
                status = 0;
                OnStop();
            }


        }
    }

    protected virtual void OnStop()
    {

    }


    protected bool HasDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath)
            return false;

        if (agent.velocity.sqrMagnitude != 0f)
            return false;

        agent.isStopped = true;
        return true;

    }

}
