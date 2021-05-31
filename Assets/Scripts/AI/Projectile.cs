using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
            transform.GetChild(0).gameObject.SetActive(true);
        }
          
    }
    private void Update()
    {
        _rigidBody.velocity = transform.right * _speed;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Health -= _damage;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<AI>().health -= _damage * 0.2f;
        }
        if (!other.gameObject.CompareTag("Projectile")
            && other.gameObject.name != ("DetectionZone"))
        {
            Effect.Projectile((other.GetContact(0).point));
            Destroy(gameObject);
        }
    }
}