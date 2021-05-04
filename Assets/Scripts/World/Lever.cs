using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Animator _animator;

    public GameObject[] doors;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void SwitchLever()
    {
        if (_animator.GetInteger("Lever") == 1)
        {
            foreach(GameObject door in doors)
                door.GetComponent<Door>().OpenOrCloseDoor();
            if(_animator.GetCurrentAnimatorStateInfo(0)
                .IsName(gameObject.name.Split(' ')[0] + "UseOff"))
            {
                _animator.Play(gameObject.name.Split(' ')[0] + "InOn");
            } 
            else
            {
                _animator.Play(gameObject.name.Split(' ')[0] + "InOff");
            } 
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