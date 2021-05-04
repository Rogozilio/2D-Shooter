using System.Collections;
using System.Collections.Generic;
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
        Vector2 S = gameObject.GetComponent<SpriteRenderer>().size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
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
    }
    private void CloseDoor()
    {
        _animator.SetBool("Door", false);
    }
}
