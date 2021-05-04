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
            = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }
}
