using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spitter : AI
{

    public GameObject Projectile;
    [Range(1, 10)]
    public float SpeedProjectile;
    private void OnEnable()
    {
        baseStopDisntance = agent.stoppingDistance;

        soundDetection  = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundDetection");
        soundAttack     = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundAttack");
        soundDead       = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundDead");
        soundEnemy      = Resources.Load<AudioClip>("Sounds/AI/Spitter/soundSpitter" 
                        + ((Random.Range(0, 2) == 0) ? "1" : "2"));

        PlaySound(soundEnemy, true, false, Random.Range(1f, 4f));
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
        bullet.GetComponent<Projectile>().Damage = damage;
        PlaySound(soundAttack, false, true);
    }
    private void Dead()
    {
        if (health <= 0)
        {
            StopAllCoroutines();
            audio.Stop();
            PlaySound(soundDead, false, true);
            if (!audio.isPlaying)
                audio.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Color color = spriteRender.color;
            color.a = 1;
            spriteRender.color = color;
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
        //DrawPath();
    }
}