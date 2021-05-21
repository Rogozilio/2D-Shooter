using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioClip soundStep;
    private Vector2 moveInput;
    private Vector2 currentDirection;
    private Transform transformObject;
    private Animator anim;
    private Weapon weapon;
    private float currentSpeed;

    public float speedDeceleration;
    public float speed;
    public float speedIncrease;
    public float speedRotate;

    public GameObject Walking;
    public GameObject AttackM;

    [SerializeField]
    GameObject Attack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        soundStep = Resources.Load<AudioClip>("Sounds/Player/soundStep");

        transformObject = transform;
        currentDirection = new Vector3(1.0f, 0.0f, 0.0f);
        currentSpeed = speed;
        Attack.SetActive(false);
    }

    void Update()
    {
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
    private bool CheckReadyShot()
    {
        if (anim.GetInteger("Reload") == 0)
            return true;
        else
            return false;
    }
    private float CalculateAngleBullet()
    {
        float angle = 2f;
        if(moveInput == Vector2.zero)
            return angle;
        else
            return angle * currentSpeed;
    }

    private void SwitchWeapon(EquipWeapon currentWeapon)
    {
        weapon.inHand = currentWeapon;
        anim.SetInteger("Weapon", (int)currentWeapon);
    }

    private IEnumerator DoAttack()
    {
        Attack.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Attack.SetActive(false);
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
}
