using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBarrelExplosion : MonoBehaviour
{
    [Range(1f, 100f)]
    public float Health = 1f;
    [Range(1f, 100f)]
    public float Damage = 50f;
    [Range(1f, 10f)]
    public float ExplosionRadius = 3f;

    [ContextMenu("Explosion")]
    public void Explosion()
    {
        GetComponent<Animator>().Play("Explosion");
        GetComponent<AudioSource>().Play();

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
                    col.GetComponent<AI>().health -= Damage * 2;
                }
            }
        }

        GetComponent<CircleCollider2D>().enabled = false;
        Color color = GetComponent<SpriteRenderer>().color; 
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }
}
