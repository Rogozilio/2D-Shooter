using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damage;
    public float speed;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<AI>().Health -= damage;
        }
        if(collision.CompareTag("Box"))
        {
            collision.GetComponent<Box>().TakeDamage();
        }
        if(!collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }   
    }
}