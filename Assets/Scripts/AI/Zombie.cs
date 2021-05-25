using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : AI
{
    void OnEnable()
    {
        baseStopDisntance
            = (GetComponent<CircleCollider2D>().radius 
            + target.GetComponent<CircleCollider2D>().radius);

        soundEnemy      = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundZombie");
        soundAttack     = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundAttack");
        soundDetection  = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDetection");
        soundDead       = Resources.Load<AudioClip>("Sounds/AI/Zombie/soundDead");

        PlaySound(soundEnemy, true, false, Random.Range(1f, 13f));
    }
    private void Attack()
    {
        if(agent.remainingDistance <= baseStopDisntance
            && isActivePursuit && isPlayerInSight)
        {
            animator.SetInteger("Enemy", 2);
        }
    }
    private void EventAttack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            PlaySound(soundAttack, false, true);
            target.GetComponent<Player>().Health -= Damage;
        }
    }
    private void Dead()
    {
        if(Health <= 0)
        {
            StopAllCoroutines();
            audio.Stop();
            PlaySound(soundDead, false, true);
            if (!audio.isPlaying)
                audio.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            agent.enabled = false;
            enabled = false;
            animator.Play("DeadZombie");
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
                    = (GetComponent<CircleCollider2D>().radius
                    + target.GetComponent<CircleCollider2D>().radius) * 2;
            MoveToTarget();
            Attack();
        }
        Dead();
        DrawPath();
    }
}