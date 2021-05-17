using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject Up;
    private Transform transformObject;

    public HealthBar healtBar;
    // Start is called before the first frame update

    public void Start()
    {
        currentHealth = maxHealth;
        healtBar.SetMaxHealth(maxHealth);
        transformObject = this.transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Enemy>())
        {
            TakeDamage(2);
        }
        if (collision.GetComponent<Health>())
        {
            currentHealth += 25;
            Destroy(Instantiate(Up, transformObject.position, Quaternion.identity),0.2f);
            Destroy(collision.gameObject);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healtBar.SetHealth(currentHealth);
    }
}
