using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private bool        _isActivePursuit;
    private bool        _isPlayerInSight;
    private AIPursuit   _ai;
    private Transform   _targetPos;
    private Vector3     _lastVisitPos;

    public bool IsActivePursuit { get => _isActivePursuit; }

    void OnEnable()
    {
        _isPlayerInSight = true;
        _isActivePursuit = false;
        _lastVisitPos = Vector3.zero;
        _targetPos = GameObject.FindGameObjectWithTag("Player").transform;
        _ai = new AIPursuit
        {
            Agent = GetComponent<NavMeshAgent>()
        };
    }
    private void SetLastVisitPos()
    {
        if (_isPlayerInSight)
            _lastVisitPos = _targetPos.position;
        if (_ai.Agent.Raycast(_targetPos.position, out _))
            _isPlayerInSight = false;
    } 
    private void StoppingOnTargetLoss()
    {
        if (_ai.Agent.isStopped
            && !_isPlayerInSight)
        {
            _isActivePursuit = false;
        }
    }
    void Update()
    {
        if (_isActivePursuit)
        {
            SetLastVisitPos();
            _ai.Agent.SetDestination(_lastVisitPos);
            StoppingOnTargetLoss();
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            _isActivePursuit = true;
            _isPlayerInSight = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Zombie zombie = (Zombie)other.gameObject.GetComponent("Zombie");
            if (zombie.IsActivePursuit)
            {
                _isActivePursuit = true;
            }
        }
    }
}