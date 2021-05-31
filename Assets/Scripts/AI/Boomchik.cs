using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boomchik : AI
{
    public float ExplosionRadius = 4;

    void OnEnable()
    {
        baseStopDisntance
            = (GetComponent<BoxCollider2D>().size.y
               + target.GetComponent<CircleCollider2D>().radius) * 1.5f;

        soundDetection = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundDetection");
        soundEnemy = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundBoomchik");
        soundDead = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundDead");
    }

    protected override void AnimationMove()
    {
        animator.SetInteger("Enemy", 1);
        if (agent.velocity == Vector3.zero
            && animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBoomchik"))
            animator.StartPlayback();
        else
            animator.StopPlayback();
    }

    private void Attack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance
            && agent.remainingDistance != 0
            && isActivePursuit && isPlayerInSight)
        {
            agent.enabled = false;
            animator.Play("AttackBoomchik");
            animator.SetInteger("Enemy", 2);
        }
    }

    private void EventAttack()
    {
        animator.Play("ExplosionBoomchik");
        Collider2D[] collider2D
            = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, ~(1 << 8));

        if (collider2D != null)
        {
            foreach (Collider2D col in collider2D)
            {
                if (col.CompareTag("Player"))
                {
                    Ray ray = new Ray(transform.position,
                        col.transform.position - transform.position);
                    RaycastHit2D hit = Physics2D
                        .Raycast(ray.origin, ray.direction
                            , Vector2.Distance(transform.position, col.transform.position));
                    if (hit.collider.name.Substring(0, 4) != "Wall")
                    {
                        col.GetComponent<Player>().Health -= damage;
                    }
                }
                else if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<AI>().health -= damage * 1.5f;
                }
                else if (col.CompareTag("Box"))
                {
                    col.GetComponent<Box>().TakeDamage();
                }
            }
        }

        Dead();
        Color color = spriteRender.color;
        color.a = 1;
        spriteRender.color = color;
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
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            enabled = false;
        }
    }

    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            RotateAtTarget();
        }

        if (isActivePursuit)
        {
            agent.stoppingDistance
                = (GetComponent<BoxCollider2D>().size.y / 2
                   + target.GetComponent<CircleCollider2D>().radius) * 2;
            if (!audio.isPlaying)
                PlaySound(soundEnemy, true);
            MoveToTarget();
            Attack();
        }

        if (health <= 0)
        {
            EventAttack();
        }

        //DrawPath();
    }
}