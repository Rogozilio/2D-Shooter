using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHand : MonoBehaviour
{
    public int damage = 8;

    private void OnEnable()
    {
        StartCoroutine(TimeAttack(0.2f));
    }
    private IEnumerator TimeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<AI>().Health -= damage;
        }
        if (col.CompareTag("Box"))
        {
            col.GetComponent<Box>().TakeDamage();
        }
        gameObject.SetActive(false);
    }
}
