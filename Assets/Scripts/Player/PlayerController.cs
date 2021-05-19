using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;



    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public float RotSpeed;

    private Vector2 currentDirection = new Vector3(1.0f, 0.0f, 0.0f);
    private Transform transformObject;

    private Animator anim;

    public bool rezK, rezP, rezR, rezS, reload;
    public bool Rifle = false;
    public bool Shotgun = false;

    public GameObject Walking;
    public GameObject AttackM;

    [SerializeField]
    GameObject Attack;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        transformObject = this.transform;
        Attack.SetActive(false);
        
    }

    
    
    void Update()
    {
        

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector2 moveInput1 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            speed /= 2;
            moveVelocity = moveInput1.normalized * speed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Vector2 moveInput1 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            speed *= 2;
            moveVelocity = moveInput1.normalized * speed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector2 moveInput1 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            speed *= 2;
            moveVelocity = moveInput1.normalized * speed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Vector2 moveInput1 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            speed /= 2;
            moveVelocity = moveInput1.normalized * speed;
        }

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
      
        if (Input.GetKey(KeyCode.G))
        {
            
            anim.SetBool("Attack", true);
            anim.SetBool("AttackP", true);
            anim.SetBool("AttackR", true);
            anim.SetBool("AttackS", true);
            
            StartCoroutine(DoAttack());
        }
        else
        {
            anim.SetBool("Attack", false);
            anim.SetBool("AttackP", false);
            anim.SetBool("AttackR", false);
            anim.SetBool("AttackS", false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            anim.SetBool("Pistol", false);
            rezP = false;
            anim.SetBool("Rifle", false);
            rezP = false;
            anim.SetBool("Shotgun", false);
            rezP = false;
            anim.SetBool("Knife", true);
            rezK = true; 
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            anim.SetBool("Rifle", false);
            rezP = false;
            anim.SetBool("Shotgun", false);
            rezP = false;
            anim.SetBool("Knife", false);
            rezK = false;
            anim.SetBool("Pistol", true);
            rezP = true;  
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Rifle == true)
        {

            
            anim.SetBool("Shotgun", false);
            rezP = false;
            anim.SetBool("Knife", false);
            rezK = false;
            anim.SetBool("Pistol", false);
            rezP = true;
            anim.SetBool("Rifle", true);
            rezP = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && Shotgun == true)
        {

            anim.SetBool("Rifle", false);
            rezP = false;
            anim.SetBool("Knife", false);
            rezK = false;
            anim.SetBool("Pistol", false);
            rezP = false;
            anim.SetBool("Shotgun", true);
            rezP = true;
        }


        if (moveInput != 0)
        {
            //Instantiate(Walking, transformObject.position, Quaternion.identity);
            if (rezK == true)
            {
                anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("Running", false);
            }
            if (rezP == true)
            {
                anim.SetBool("RunningP", true);
            }
            else
            {
                anim.SetBool("RunningP", false);
            }
            if (rezR == true)
            {
                anim.SetBool("RunningR", true);
            }
            else
            {
                anim.SetBool("RunningR", false);
            }
            if (rezS == true)
            {
                anim.SetBool("RunningS", true);
            }
            else
            {
                anim.SetBool("RunningS", false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Rifle>())
        {
            Rifle = true;
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<Shotgun>())
        {
            Shotgun = true;
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<Object>())
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DoAttack()
    {
        Attack.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        Attack.SetActive(false);

    }

    public void EventMove()
    {
        Destroy(Instantiate(Walking, transformObject.position, Quaternion.identity), 1.8f);
    }
    public void EventAttackM()
    {
        Destroy(Instantiate(AttackM, transformObject.position, Quaternion.identity), 0.4f);
    }


}
