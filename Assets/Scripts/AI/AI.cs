using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{ 
    private bool                isFindNewPos;

    protected NavMeshAgent      agent;
    protected GameObject        target;
    protected LineRenderer      line;
    protected Animator          animator;
    protected Vector3           lastVisitPos;
    protected AudioClip         soundDetection;
    protected AudioClip         soundEnemy;
    protected AudioClip         soundAttack;
    protected AudioClip         soundDead;
    protected AudioSource       audio;
    protected float             baseSpeed;
    protected float             baseStopDisntance;
    protected bool              isActivePursuit;
    protected bool              isPlayerInSight;

    public bool IsActivePursuit { get => isActivePursuit; }
    public bool IsPlayerInSight { get => isPlayerInSight; }

    [Range(0, 1000)]
    public float Health;
    [Range(0, 100)]
    public float Damage;

    [HideInInspector]
    public bool isActiveRandomStep;
    [HideInInspector]
    public float radius = 1f;
    [HideInInspector]
    public float speed = 0.5f;
    [HideInInspector]
    public float delay = 4f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        target      = GameObject.FindGameObjectWithTag("Player");
        audio       = GetComponent<AudioSource>();
        animator    = GetComponent<Animator>();
        line        = GetComponent<LineRenderer>();

        isActivePursuit = false;
        lastVisitPos    = transform.position;
        baseSpeed       = agent.speed;

        StartCoroutine(RandomStep());
    }
    virtual protected void MoveToTarget()
    {
        agent.speed = baseSpeed;
        agent.SetDestination(SetLastVisitPos());
        AnimationMove();
        StoppingOnTargetLoss();
        animator.speed = agent.speed / baseSpeed;
    }
    protected IEnumerator RandomStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(delay - 1, delay));
            if (isActivePursuit
                || !isActiveRandomStep)
            {
                yield break;
            }
            if(Vector3.Distance(target.transform.position, transform.position) < 25)
            {
                Vector3 newPosStandstill 
                    = CheckNextPos(transform.position, radius);
                agent.speed = speed;
                agent.stoppingDistance = 0;
                agent.SetDestination(newPosStandstill);
                animator.SetInteger("Enemy", 1);
                animator.speed = agent.speed / baseSpeed;
            }
            yield return new WaitUntil(()
                => agent.velocity == Vector3.zero);
            animator.SetInteger("Enemy", 0);
        }
    }
    private IEnumerator CheckPlayerInSight(float time = 2f)
    {
        yield return new WaitForSeconds(time);
        if (agent.Raycast(target.transform.position, out _))
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
    protected void PlaySound(AudioClip clip
        , bool isLoop = false
        , bool isPlayOnShot = false
        , float time = 0f)
    {
        if(clip != null)
        {
            audio.time = time;
            
            if (isPlayOnShot)
                audio.PlayOneShot(clip);
            else
            {
                audio.loop = isLoop;
                audio.clip = clip;
                audio.Play();
            } 
        }
    }
    protected void RotateAtTarget(Vector3? eye = null)
    {
        float signedAngle = Vector2.SignedAngle(eye ?? transform.up
            , agent.steeringTarget - transform.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = transform.eulerAngles;
            angles.z += signedAngle;
            transform.eulerAngles = angles;
        }
    }
    virtual protected void AnimationMove()
    {
        if (agent.velocity == Vector3.zero )
            animator.SetInteger("Enemy", 0);
        else
            animator.SetInteger("Enemy", 1);
    }
    private Vector3 CheckNextPos(Vector3 oldPos, float maxLenghtStep)
    {
        Vector3 newPos = oldPos + new Vector3(Random.Range(0, maxLenghtStep)
                , Random.Range(0, maxLenghtStep));
        while (!agent.CalculatePath(newPos, agent.path))
        {
            maxLenghtStep -= 0.01f;
            newPos = oldPos + new Vector3(Random.Range(0, maxLenghtStep)
                , Random.Range(0, maxLenghtStep));
            if(maxLenghtStep <= 0)
            {
                return oldPos;
            }
        }
        return newPos;
    }
    private Vector3 SetLastVisitPos()
    {
        if (isPlayerInSight)
        {
            lastVisitPos = target.transform.position;
            agent.stoppingDistance = baseStopDisntance;
            isFindNewPos = true;
        }
        if (isPlayerInSight
            && agent.Raycast(target.transform.position, out _))
        {
            StartCoroutine(CheckPlayerInSight());
        }
        if (!isPlayerInSight
            && isFindNewPos)
        {
            lastVisitPos = CheckNextPos(lastVisitPos, radius);
            agent.stoppingDistance = 0;
            isFindNewPos = false;
        } 
        return lastVisitPos;
    }
    private void StoppingOnTargetLoss()
    {
        if (agent.velocity == Vector3.zero
            && !isPlayerInSight)
        {
            isActivePursuit = false;
            //StartCoroutine(RandomStep());
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        //созвать ближайших врагов
        if (other.CompareTag("Enemy"))
        {
            AI enemy = other.gameObject.GetComponent<AI>();
            if (enemy.IsActivePursuit
                && enemy.IsPlayerInSight
                && !isPlayerInSight)
            {
                isActivePursuit = true;
                isPlayerInSight = true;
            }
        }
        //обнаружение игрока
        if (other.CompareTag("Player")
            && (!isActivePursuit || !isPlayerInSight))
        {
            Ray ray = new Ray(transform.position,
                   other.transform.position - transform.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (other.CompareTag(hit.collider.gameObject.tag))
            {
                PlaySound(soundDetection, false, true);
                isActivePursuit = true;
                isPlayerInSight = true;
            }
        }
    }
}