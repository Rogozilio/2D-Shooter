using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private bool isOpenDoor = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        CloseDoor();
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
