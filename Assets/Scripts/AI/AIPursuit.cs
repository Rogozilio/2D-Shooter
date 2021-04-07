using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIPursuit : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Vector3      lastVisitPos;
    protected bool         IsActivePursuit;
    protected bool         IsPlayerInSight;

    public bool IsActiveRandomStep;
    public Vector3 SetLastVisitPos(Transform target)
    {
        if (IsPlayerInSight)
            lastVisitPos = target.position;
        if (agent.Raycast(target.position, out _))
            IsPlayerInSight = false;
        return lastVisitPos;
    }
    public void StoppingOnTargetLoss()
    {
        if (agent.velocity == Vector3.zero
            && !IsPlayerInSight)
        {
            IsActivePursuit = false;
            StartCoroutine(RandomStep());
        }
    }
    public IEnumerator RandomStep(float radius = 0.5f
        , float speed = 1f, float delay = 2f)
    {
        float baseSpeed = agent.speed;
        while (true)
        {
            if (IsActivePursuit
                || !IsActiveRandomStep)
            {
                agent.speed = baseSpeed;
                yield break;
            } 
            Vector3 _newPosStandstill = transform.position
                + new Vector3(Random.Range(-radius, radius)
                , Random.Range(-radius, radius), 0);
            agent.speed = speed;
            agent.SetDestination(_newPosStandstill);
            yield return new WaitUntil(()
                => agent.velocity == Vector3.zero);
            yield return new WaitForSeconds(Random.Range(0, delay));
        }
    }
}