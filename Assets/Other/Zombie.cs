using UnityEngine;
using UnityEngine.AI;

public class Zombie : AIPursuit
{
    private Transform _target;
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        IsActivePursuit = false;
        lastVisitPos = transform.position;

        _target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(RandomStep());
    }
    void Update()
    {
        if (IsActivePursuit)
        {
            agent.SetDestination(SetLastVisitPos(_target));
            StoppingOnTargetLoss();
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            IsActivePursuit = true;
            IsPlayerInSight = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Zombie zombie = (Zombie)other.gameObject.GetComponent("Zombie");
            if (zombie.IsActivePursuit)
            {
                IsActivePursuit = true;
            }
        }
    }
}