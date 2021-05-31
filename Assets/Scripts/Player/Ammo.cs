using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public int damage;
    public float speed;

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<AI>().health -= damage;
            Effect.Blood(collision.GetContact(0).point);
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Box>().TakeDamage();
        }
        if (collision.gameObject.CompareTag("RedBurrel"))
        {
            collision.gameObject.GetComponent<RedBarrelExplosion>().Explosion();
        }
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}