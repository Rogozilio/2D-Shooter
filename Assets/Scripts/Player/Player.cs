using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audio;
    private AudioClip soundStep1;
    private AudioClip soundStep2;
    private AudioClip soundStep3;
    private AudioClip soundStep4;
    private AudioClip soundStep5;
    private Vector2 moveInput;
    private Vector2 currentDirection;
    private Transform transformObject;
    private HealthBar healthBar;
    private Animator anim;
    public Weapon weapon;
    private float currentSpeed;
    private bool isActiveKeyE;

    public GameObject HealthBar;
    public Sprite spriteknife;
    public Sprite spriteAmmoPistol;
    public Sprite spriteAmmoShotgun;
    public Sprite spriteAmmoRifle;
    [SerializeField]
    private float health;
    public float speedDeceleration;
    public float speed;
    public float speedIncrease;
    public float speedRotate;
    public bool hasRifle;
    public bool hasShotgun;

    public float Health
    {
        get => healthBar.Health;
        set => healthBar.Health = value;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        weapon = GetComponent<Weapon>();
        healthBar = HealthBar.GetComponent<HealthBar>();
        soundStep1 = Resources.Load<AudioClip>("Sounds/Player/soundStep1");
        soundStep2 = Resources.Load<AudioClip>("Sounds/Player/soundStep2");
        soundStep3 = Resources.Load<AudioClip>("Sounds/Player/soundStep3");
        soundStep4 = Resources.Load<AudioClip>("Sounds/Player/soundStep4");
        soundStep5 = Resources.Load<AudioClip>("Sounds/Player/soundStep5");

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
            weapon.ammoImage.sprite = spriteknife;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(EquipWeapon.Pistol);
            weapon.ammoImage.sprite = spriteAmmoPistol;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hasRifle)
        {
            SwitchWeapon(EquipWeapon.Shotgun);
            weapon.ammoImage.sprite = spriteAmmoShotgun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && hasShotgun)
        {
            SwitchWeapon(EquipWeapon.Rifle);
            weapon.ammoImage.sprite = spriteAmmoRifle;
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
        if (weapon.inHand == EquipWeapon.Knife)
            weapon.ammoCount.text = "";
        else
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
        if (collision.CompareTag("PickUp"))
        {
            Item item = collision.GetComponent<PickUp>().Item;
            switch(item)
            {
                case Item.health:
                Health += 75f;
                break;
                case Item.Shotgun:
                hasShotgun = true;
                break;
                case Item.Rifle:
                hasRifle = true;
                break;
                case Item.PistolAmmo:
                weapon.allAmmoP += 20;
                break;
                case Item.ShotgunAmmo:
                weapon.allAmmoS += 12;
                break;
                case Item.RifleAmmo:
                weapon.allAmmoR += 30;
                break;
            }
        }
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
        if (moveInput == Vector2.zero)
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
        else if (moveInput.x < 0)
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
    public void EventSoundStep()
    {
        AudioClip soundStep = null;
        switch(UnityEngine.Random.Range(0, 5))
        {
            case 0: soundStep = soundStep1; break;
            case 1: soundStep = soundStep2; break;
            case 2: soundStep = soundStep3; break;
            case 3: soundStep = soundStep4; break;
            case 4: soundStep = soundStep5; break;
        }
        audio.PlayOneShot(soundStep);
    }
}