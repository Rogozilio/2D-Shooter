using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPursuit
{
    private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get => agent;
        set
        {
            agent = value;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
}
