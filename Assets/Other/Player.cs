using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    [Range(1f, 100f)]
    public float Health;
    public float Speed;
    void Start()
    {
        Speed = 5;
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        _rigidBody.velocity 
            = new Vector2(Input.GetAxis("Horizontal") * Speed
            , Input.GetAxis("Vertical") * Speed);
    }
}
