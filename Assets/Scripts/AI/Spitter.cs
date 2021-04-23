using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : AI
{
    private AudioSource _audio;
    private AudioClip _soundDetection;
    private AudioClip _soundAttack;
    private AudioClip _soundDead;
    private AudioClip _soundSpitter1;
    private AudioClip _soundSpitter2;

    public GameObject Projectile;
    [Range(1, 10)]
    public float SpeedProjectile;
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        isActivePursuit = false;
        lastVisitPos = transform.position;
        baseSpeed = agent.speed;
        baseStopDisntance = agent.stoppingDistance;

        _audio = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

        _soundDetection = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundDetection");
        _soundAttack = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundAttack");
        _soundDead = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundDead");
        _soundSpitter1 = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundSpitter1");
        _soundSpitter2 = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundSpiiter2");

        SoundVoiceSpitter();
        StartCoroutine(RandomStep());
    }
    private void SoundVoiceSpitter()
    {
        _audio.clip = (Random.Range(0, 2) == 0)? _soundSpitter1 : _soundSpitter2;
        _audio.loop = true;
        _audio.time = Random.Range(1f, 4f);
        _audio.Play();
    }
    private void Attack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance
            && isActivePursuit && isPlayerInSight)
        {
            animator.SetInteger("Enemy", 2);
            RotateAtTarget(transform.right);
        }
    }
    private void EventAttack()
    {
        float radius = 0.3f;
        Vector3 startPosProjectile = transform.position 
            + Vector3.ClampMagnitude(transform.right, radius);
        GameObject bullet = Instantiate(Projectile, startPosProjectile, transform.rotation);
        bullet.GetComponent<Projectile>().Speed = SpeedProjectile;
        bullet.GetComponent<Projectile>().Damage = Damage;
        _audio.PlayOneShot(_soundAttack);
    }
    private void Dead()
    {
        if (Health <= 0)
        {
            StopAllCoroutines();
            _audio.Stop();
            _audio.PlayOneShot(_soundDead);
            if (!_audio.isPlaying)
                _audio.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            agent.enabled = false;
            enabled = false;
            animator.Play("DeadSpitter");
        }
    }
    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            RotateAtTarget(transform.right);
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