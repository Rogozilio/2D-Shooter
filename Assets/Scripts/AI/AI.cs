using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    protected NavMeshAgent      agent;
    protected GameObject        target;
    protected SpriteRenderer    sprite;
    protected LineRenderer      line;
    protected Animator          animator;
    protected Vector3           lastVisitPos;
    protected float             baseSpeed;
    protected float             baseStopDisntance;
    protected bool              isActivePursuit;
    protected bool              isPlayerInSight;
    private bool                isActiveNewPos;

    public bool IsActivePursuit { get => isActivePursuit; }
    public bool IsPlayerInSight { get => isPlayerInSight; }

    [Range(0, 100)]
    public float Health = 100;
    [Range(0, 100)]
    public float Damage = 10;

    [HideInInspector]
    public bool isActiveRandomStep;
    [HideInInspector]
    public float radius = 1f;
    [HideInInspector]
    public float speed = 0.5f;
    [HideInInspector]
    public float delay = 4f;

    protected void MoveToTarget()
    {
        agent.speed = baseSpeed;
        agent.SetDestination(SetLastVisitPos(target.transform));
        AnimationMove();
        StoppingOnTargetLoss();
        animator.speed = agent.speed / baseSpeed;
        //agent.stoppingDistance = baseStopDisntance;
    }
    protected IEnumerator RandomStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(delay - 1, delay));
            if (isActivePursuit
                || !isActiveRandomStep)
            {
                //agent.stoppingDistance 
                //    = (GetComponent<CircleCollider2D>().radius
                //    + target.GetComponent<CircleCollider2D>().radius) * 2;
                yield break;
            }
            Vector3 newPosStandstill 
                = CheckNextPos(transform.position, -radius, radius);
            agent.speed = speed;
            agent.stoppingDistance = 0;
            agent.SetDestination(newPosStandstill);
            AnimationMove();
            animator.SetInteger("Enemy", 1);
            animator.speed = agent.speed / baseSpeed;
            yield return new WaitUntil(()
                => agent.velocity == Vector3.zero);
            animator.SetInteger("Enemy", 0);
        }
    }
    private IEnumerator CheckPlayerInSight(Transform target, float time = 2f)
    {
        yield return new WaitForSeconds(time);
        if (agent.Raycast(target.position, out _))
        {
            isPlayerInSight = false;
        }
    }
    protected void DrawPath()
    {
        if(agent.hasPath)
        {
            line.positionCount = agent.path.corners.Length;
            line.SetPositions(agent.path.corners);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }
    
    protected void RotateAtTarget(Vector3? eye = null)
    {
        float signedAngle = Vector2.SignedAngle(eye ?? transform.up, agent.steeringTarget - transform.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = transform.eulerAngles;
            angles.z += signedAngle;
            transform.eulerAngles = angles;
        }
    }
    private void AnimationMove()
    {
        if (agent.velocity == Vector3.zero)
            animator.SetInteger("Enemy", 0);
        else
            animator.SetInteger("Enemy", 1);
    }
    private Vector3 CheckNextPos(Vector3 oldPos, float randomMin, float randomMax)
    {
        Vector3 newPos = oldPos + new Vector3(Random.Range(randomMin, randomMax)
                , Random.Range(randomMin, randomMax)); ;
        while (!agent.CalculatePath(newPos, agent.path))
        {
            newPos = oldPos + new Vector3(Random.Range(randomMin, randomMax)
                , Random.Range(randomMin, randomMax));
        }
        return newPos;
    }
    private Vector3 SetLastVisitPos(Transform target)
    {
        if (isPlayerInSight)
        {
            lastVisitPos = target.position;
            agent.stoppingDistance = baseStopDisntance;
            isActiveNewPos = true;
        } 
        if (isPlayerInSight
            && agent.Raycast(target.position, out _))
        {
            StartCoroutine(CheckPlayerInSight(target));
        }
        if (!isPlayerInSight
            && isActiveNewPos)
        {
            lastVisitPos = CheckNextPos(lastVisitPos, 0f, 1f);
            agent.stoppingDistance = 0;
            isActiveNewPos = false;
        } 
        return lastVisitPos;
    }
    private void StoppingOnTargetLoss()
    {
        if (agent.velocity == Vector3.zero
            && !isPlayerInSight)
        {
            isActivePursuit = false;
            StartCoroutine(RandomStep());
        }
    }
}