using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHand : MonoBehaviour
{
    public int damage = 8;
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator TimeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    private IEnumerator PushEnemy(Transform enemy, float pushForce)
    {
        while (true)
        {
            if(pushForce <= 0)
                yield break;
            enemy.position -= (enemy.up * (pushForce * Time.deltaTime));
            pushForce -= 0.05f;
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<AI>().health -= damage;
            Effect.Blood(transform.position);
            StartCoroutine(PushEnemy(col.transform, 6f));
        }
        if (col.CompareTag("Box"))
        {
            col.GetComponent<Box>().TakeDamage();
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
