using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private Rigidbody2D _rigidBody;
    [HideInInspector]
    public float Speed {set => _speed = value;}
    [HideInInspector]
    public float Damage { set => _damage = value; }
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _rigidBody.velocity = transform.right * _speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Health -= _damage;
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<AI>().Health -= _damage * 0.2f;
        }
        if (!other.CompareTag("Projectile")
            && other.name != ("DetectionZone"))
        {
            Destroy(gameObject);
        }
    }
}