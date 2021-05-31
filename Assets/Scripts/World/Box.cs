using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject item;

    [ContextMenu("BrokenBox")]
    public void TakeDamage()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("Box", false);
        GetComponent<AudioSource>().Play();
        if (item != null)
            Instantiate(item, transform.position, Quaternion.identity);
        transform.position
            = new Vector3(transform.position.x
            , transform.position.y, transform.position.z + 0.1f);
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }
}
