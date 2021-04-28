using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator _animator;

    public GameObject door;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void SwitchLever()
    {
        if(_animator.GetInteger("Lever") == 1)
        {
            door.GetComponent<Door>().OpenOrCloseDoor();
            _animator.SetInteger("Lever", 2);
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetInteger("Lever", 0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        _animator.SetInteger("Lever", 1);
    }
}
