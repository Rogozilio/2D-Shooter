using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private AIPursuit _ai;
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _ai = new AIPursuit();
        _ai.Agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _ai.Agent.SetDestination(_target.position);
    }
}
