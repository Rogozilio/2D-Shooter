using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : AI
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

        _audio = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

        isActivePursuit = false;
        lastVisitPos = transform.position;
        baseSpeed = agent.speed;
        baseStopDisntance
            = (GetComponent<CircleCollider2D>().radius 
            + target.GetComponent<CircleCollider2D>().radius) * 2;

        _soundZombie  = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundZombie");
        _soundAttack = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundAttack");
        _soundDetection = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDetection");
        _soundDead = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDead");

        SoundVoiceZombie();
        StartCoroutine(RandomStep());
    }
    private void SoundVoiceZombie()
    {
        _audio.clip = _soundZombie;
        _audio.loop = true;
        _audio.time = Random.Range(1f, 13f);
        _audio.Play();
    }
    private void Attack()
    {
        if(agent.remainingDistance <= 0.7
            && isActivePursuit && isPlayerInSight)
        {
            animator.SetInteger("Enemy", 2);
        }
    }
    private void EventAttack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _audio.PlayOneShot(_soundAttack);
            target.GetComponent<Player>().Health -= Damage;
        }
    }
    private void Dead()
    {
        if(Health <= 0)
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
            agent.stoppingDistance
                    = (GetComponent<CircleCollider2D>().radius
                    + target.GetComponent<CircleCollider2D>().radius) * 2;
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
            AI enemy= other.gameObject.GetComponent<AI>();
            if (enemy.IsActivePursuit
                && enemy.IsPlayerInSight
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