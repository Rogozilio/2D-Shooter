using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private bool _isKeyEActive = false;
    [Range(1f, 100f)]
    public float Health;
    public float Speed;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _rigidBody.velocity
            = new Vector2(Input.GetAxis("Horizontal") * Speed
            , Input.GetAxis("Vertical") * Speed);
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isKeyEActive = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isKeyEActive)
        {
            if (collision.gameObject.tag == "Lamp")
            {
                collision.gameObject
                .GetComponent<Lamp>().ActiveLamp();
            }
            else if (collision.gameObject.tag == "Lever")
            {
                collision.gameObject
                    .GetComponent<Lever>().SwitchLever();
            }
            _isKeyEActive = false;
        }
    }
}