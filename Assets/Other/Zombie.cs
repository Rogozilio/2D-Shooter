using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : AIPursuit
{
    private AudioClip   _soundZombie;
    private AudioClip   _soundAttack;
    private AudioClip   _soundDetection;
    private AudioClip   _soundDead;
    private AudioSource _audio;
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        isActivePursuit = false;
        lastVisitPos = transform.position;
        baseSpeed = agent.speed;

        _audio = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        agent.stoppingDistance 
            = (GetComponent<CircleCollider2D>().radius 
            + target.GetComponent<CircleCollider2D>().radius) * 2;

        _soundZombie  = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundZombie");
        _soundAttack = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundAttack");
        _soundDetection = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDetection");
        _soundDead = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDead");

        StartCoroutine(RandomStep());
        StartCoroutine(SoundVoiceZombie());
    }
    private IEnumerator SoundVoiceZombie()
    {
        _audio.clip = _soundZombie;
        _audio.loop = true;
        _audio.time = Random.Range(1f, 13f);
        _audio.Play();
        yield return new WaitForSeconds(1f);
    }
    private void Attack()
    {
        if(agent.remainingDistance <= 0.7
            && isActivePursuit && isPlayerInSight)
        {
            animator.SetInteger("Zombie", 2);
        }
    }
    private void EventAttack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _audio.PlayOneShot(_soundAttack);
            target.GetComponent<Player>().health -= damage;
        }
    }
    private void Dead()
    {
        if(health <= 0)
        {
            StopAllCoroutines();
            _audio.Stop();
            _audio.PlayOneShot(_soundDead);
            if(!_audio.isPlaying)
                _audio.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            agent.enabled = false;
            enabled = false;
            animator.Play("DeadZombie");
        }
    }
    void Update()
    {
        if(agent.velocity != Vector3.zero)
        {
            RotateAtTarget();
        }
        if (isActivePursuit)
        {
            MoveToTarget();
            Attack();
        }
        Dead();
        DrawPath();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        //созвать ближайших врагов
        if (other.CompareTag("Enemy"))
        {
            Zombie zombie = (Zombie)other.gameObject.GetComponent("Zombie");
            if (zombie.isActivePursuit
                && zombie.isPlayerInSight
                && !isPlayerInSight)
            {
                isActivePursuit = true;
                isPlayerInSight = true;
            }
        }
        //обнаружение игрока
        if (other.CompareTag("Player") && !isPlayerInSight)
        {
            Ray ray = new Ray(transform.position,
                   other.transform.position - transform.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (other.CompareTag(hit.collider.gameObject.tag))
            {
                _audio.PlayOneShot(_soundDetection);
                isActivePursuit = true;
                isPlayerInSight = true;
            }
        }
    }
}