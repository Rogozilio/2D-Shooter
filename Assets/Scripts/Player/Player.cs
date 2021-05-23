using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioClip soundStep;
    private Vector2 moveInput;
    private Vector2 currentDirection;
    private Transform transformObject;
    private HealthBar healthBar;
    private Animator anim;
    private Weapon weapon;
    private float currentSpeed;
    private bool isActiveKeyE;
    
    public GameObject HealthBar;
    [SerializeField]
    private float health;
    public float speedDeceleration;
    public float speed;
    public float speedIncrease;
    public float speedRotate;

    public float Health { get => healthBar.Health; 
        set => healthBar.Health = value; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        healthBar = HealthBar.GetComponent<HealthBar>();
        soundStep = Resources.Load<AudioClip>("Sounds/Player/soundStep");
        
        Health = health;
        currentSpeed = speed;
        isActiveKeyE = false;
        transformObject = transform;
        currentDirection = new Vector3(1.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        CalculateAnimFeet();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(EquipWeapon.Knife);
            weapon.ammoCount.text = " ";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(EquipWeapon.Pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weapon.hasRifle)
        {
            SwitchWeapon(EquipWeapon.Shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && weapon.hasShotgun)
        {
            SwitchWeapon(EquipWeapon.Rifle);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetInteger("Reload"
                , (int)weapon.Reload(weapon.inHand));
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("Attack", true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActiveKeyE = true;
        }
        weapon.Shot(CheckReadyShot(), CalculateAngleBullet());
        weapon.ammoCount.text = weapon.CurrentAmmo + " / " + weapon.AllAmmo;
    }
    private void FixedUpdate()
    {
        SpeedDeceleration();
        SpeedIncrease();
        LookAtCursor();
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rifle"))
        {
            weapon.hasRifle = true;
        }
        if (collision.CompareTag("Shotgun"))
        {
            weapon.hasShotgun = true;
        }
        //if (collision.CompareTag("PistolAmmo"))
        //{
        //    allAmmoP += 10;
        //}
        //if (collision.CompareTag("RifleAmmo"))
        //{
        //    allAmmoR += 20;
        //}
        //if (collision.CompareTag("ShotgunAmmo"))
        //{
        //    allAmmoS += 15;
        //}
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isActiveKeyE)
        {
            if (collision.CompareTag("Lamp"))
            {
                collision.gameObject
                .GetComponent<Lamp>().ActiveLamp();
            }
            else if (collision.CompareTag("Lever"))
            {
                collision.gameObject
                    .GetComponent<Lever>().SwitchLever();
            }
            isActiveKeyE = false;
        }
    }
    private bool CheckReadyShot()
    {
        if (anim.GetInteger("Reload") == 0
            && !anim.GetBool("Attack"))
            return true;
        else
            return false;
    }
    private float CalculateAngleBullet()
    {
        float angle = 2f;
        if (moveInput == Vector2.zero)
            return angle;
        else
            return angle * currentSpeed;
    }

    private void SwitchWeapon(EquipWeapon currentWeapon)
    {
        weapon.inHand = currentWeapon;
        anim.SetInteger("Weapon", (int)currentWeapon);
    }

    private void CalculateAnimFeet()
    {
        if(moveInput == Vector2.zero)
        {
            anim.SetInteger("Feet", 0);
            return;
        }
        
        float angle = Vector2.Angle(moveInput, transform.up);

        if (angle < 45)
        {
            anim.SetInteger("Feet", 2);
        }
        else if (angle > 135)
        {
            anim.SetInteger("Feet", 2);
        }
        else if(moveInput.x < 0)
        {
            anim.SetInteger("Feet", 3);
        }
        else
        {
            anim.SetInteger("Feet", 4);
        }
    }
    private void SpeedIncrease()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = speedIncrease;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = speed;
        }
    }
    private void SpeedDeceleration()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            currentSpeed = speedDeceleration;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            currentSpeed = speed;
        }
    }
    private void LookAtCursor()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        currentDirection = Vector2.Lerp(currentDirection, direction, Time.fixedDeltaTime * speedRotate);
        transformObject.up = currentDirection;
    }
    public void EventEndReload()
    {
        anim.SetInteger("Reload", 0);
    }
    public void EventAttackknife()
    {
        float sizeX = 0.8f;
        float sizeY = 1.4f;
        Collider2D[] collider2D
           = Physics2D.OverlapBoxAll(transform.position + transform.up * sizeX
           , new Vector2(sizeX, sizeY), 0, ~(1 << 8));
        if (collider2D != null)
        {
            foreach (Collider2D col in collider2D)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<AI>().Health -= weapon.damageKnife;
                }
                if (col.CompareTag("Box"))
                {
                    col.GetComponent<Box>().TakeDamage();
                }
            }
        }
    }
    public void EventAttackHand()
    {
        GetComponentInChildren<AttackHand>(true).gameObject.SetActive(true);
    }
    public void EventEndAttack()
    {
        anim.SetBool("Attack", false);
    }
}