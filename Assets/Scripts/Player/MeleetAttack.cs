using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleetAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamageM(damageM);
        }
    }
}
