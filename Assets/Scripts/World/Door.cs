using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private bool isOpenDoor = false;
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        CloseDoor();
    }
    private void Update()
    {
        gameObject.GetComponent<BoxCollider2D>().size 
            = gameObject.GetComponent<SpriteRenderer>().size;
    }
    public void OpenOrCloseDoor()
    {
        isOpenDoor = !isOpenDoor;
        if (isOpenDoor)
            OpenDoor();
        else
            CloseDoor();
    }
    private void OpenDoor()
    {
        _animator.SetBool("Door", true);
        GetComponent<ShadowCaster2D>().enabled = false;
    }
    private void CloseDoor()
    {
        _animator.SetBool("Door", false);
        GetComponent<ShadowCaster2D>().enabled = true;
    }
}
