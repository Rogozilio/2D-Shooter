using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DetectionInSight : MonoBehaviour
{
    private Transform parentTransform;
    private void OnEnable()
    {
        parentTransform = GetComponentsInParent<Transform>()[1];
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")
            || other.CompareTag("Box")
            || other.CompareTag("RedBurrel")
            || other.CompareTag("Projectile")
            || other.CompareTag("PickUp"))
        {
            Ray ray = new Ray(parentTransform.position, other.transform.position - parentTransform.position);
            RaycastHit2D[] hits = Physics2D
                .RaycastAll(ray.origin, ray.direction
                    , Vector2.Distance(parentTransform.position, other.transform.position));
            Color color = other.gameObject.GetComponent<SpriteRenderer>().color;
            if (hits.Length == 1)
            {
                color.a = 1;
                if (other.CompareTag("Projectile"))
                {
                    other.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                color.a = 0;
                if (other.CompareTag("Projectile"))
                {
                    other.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            other.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")
            || other.CompareTag("Box")
            || other.CompareTag("RedBurrel")
            || other.CompareTag("Projectile")
            || other.CompareTag("PickUp"))
        {
            Color color = other.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            other.gameObject.GetComponent<SpriteRenderer>().color = color;
            if (other.CompareTag("Projectile"))
            {
                other.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}