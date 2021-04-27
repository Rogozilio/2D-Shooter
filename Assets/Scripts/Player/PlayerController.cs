using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public float RotSpeed;

    private Vector2 currentDirection = new Vector3(1.0f, 0.0f, 0.0f);
    private Transform transformObject;

    private Animator anim;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transformObject = this.transform;
    }
    
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 objectPos = transformObject.position;

        Vector2 direction = mousePos - objectPos;
        direction.Normalize();

        currentDirection = Vector2.Lerp(currentDirection, direction, Time.deltaTime * RotSpeed);
        transformObject.up = currentDirection;

        

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        var moveInput = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");
        if (moveInput == 0)
        {
            anim.SetBool("Running", false);
        }
        else
        {
            anim.SetBool("Running", true);
        }
    }
}
