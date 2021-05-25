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

        soundDetection  = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundDetection");
        soundEnemy      = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundBoomchik");
        soundDead       = Resources.Load<AudioClip>("Sounds/AI/Boomchik/soundDead");
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
                    col.GetComponent<Player>().Health -= Damage;
                }
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<AI>().Health -= Damage;
                }
                if(col.CompareTag("Box"))
                {
                    col.GetComponent<Box>().TakeDamage();
                }
            }
        }
        Dead();
    }
    private void Dead()
    {
        if (Health <= 0)
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
        if (Health <= 0)
        {
            EventAttack();
        }
        DrawPath();
    }
}
