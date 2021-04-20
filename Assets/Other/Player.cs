using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    [Range(1f, 100f)]
    public float health;
    public float speed;
    void Start()
    {
        speed = 5;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidBody.velocity 
            = new Vector2(Input.GetAxis("Horizontal") * speed
            , Input.GetAxis("Vertical") * speed);
    }
}
