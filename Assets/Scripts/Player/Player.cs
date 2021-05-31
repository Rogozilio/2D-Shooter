using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    
    [HideInInspector]
    public bool isActiveKeyE;
    [FormerlySerializedAs("Info")]
    public Image black;
    public Image info;
    [FormerlySerializedAs("HealthBar")]
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

    private void Awake()
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
        black.enabled = true;
        currentSpeed = speed;
        isActiveKeyE = false;
        transformObject = transform;
        currentDirection = new Vector3(1.0f, 0.0f, 0.0f);
        LoadData();
        SwitchWeapon(weapon.inHand);
        StartCoroutine(BlinkHealth());
    }

    private void Update()
    {
        CalculateAnimFeet();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(EquipWeapon.Knife);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(EquipWeapon.Pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hasShotgun)
        {
            SwitchWeapon(EquipWeapon.Shotgun);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && hasRifle)
        {
            SwitchWeapon(EquipWeapon.Rifle);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetInteger("Reload"
                , (int)weapon.Reload(weapon.inHand));
        }
        else if (Input.GetMouseButtonDown(0) && weapon.inHand == EquipWeapon.Knife
                 || Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("Attack");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isActiveKeyE = true;
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            info.gameObject.SetActive(true);
        }
        else
        {
            info.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.U))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKey(KeyCode.I))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene(3);
        }
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(4);
        }
        if (Input.GetKey(KeyCode.L))
        {
            SaveLoad saveData = new SaveLoad();
            saveData.ResetData();
        }
        if (weapon.Shot(CheckReadyShot(), CalculateAngleBullet()))
        {
            anim.SetTrigger("Shot");
        }
        if (weapon.inHand == EquipWeapon.Knife)
            weapon.ammoCount.text = "";
        else
            weapon.ammoCount.text = weapon.CurrentAmmo + " / " + weapon.AllAmmo;
    }
    private void FixedUpdate()
    {
        Dead();
        ChangeSpeed();
        LookAtCursor();
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float distance = Vector2.Distance(Vector2.zero, moveInput);
        if (distance > 1)
        {
            moveInput = Vector2.Lerp(Vector2.zero, moveInput, 1f / distance);
        }
        rb.MovePosition(rb.position + moveInput * (currentSpeed * Time.fixedDeltaTime));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {
            Item item = collision.gameObject.GetComponent<PickUp>().Item;
            switch (item)
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
                isActiveKeyE = false;
            }
            // else if (collision.CompareTag("Lever"))
            // {
            //     collision.gameObject
            //         .GetComponent<Lever>().SwitchLever();
            //     isActiveKeyE = false;
            // }
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator BlinkHealth()
    {
        while (true)
        {
            yield return new WaitWhile(() => Health > 30);
            float alpha = 0.1f;
            Color color = healthBar.GetComponentsInChildren<Image>()[3].color;
            if (color.a > 0)
                color.a -= alpha;
            else
                color.a = 1;
            healthBar.GetComponentsInChildren<Image>()[3].color = color;
            yield return new WaitForSeconds(0.05f);
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
        switch (currentWeapon)
        {
            case EquipWeapon.Knife: weapon.ammoImage.sprite = spriteknife; break;
            case EquipWeapon.Pistol: weapon.ammoImage.sprite = spriteAmmoPistol; break;
            case EquipWeapon.Shotgun: weapon.ammoImage.sprite = spriteAmmoShotgun; break;
            case EquipWeapon.Rifle: weapon.ammoImage.sprite = spriteAmmoRifle;break;
        }
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

        if (currentSpeed == speedDeceleration)
        {
            anim.SetFloat("Speed", 0.3f);
        }
        else if(currentSpeed == speed)
            anim.SetFloat("Speed", 0.7f);
        else
            anim.SetFloat("Speed", 1f);
    }
    private void ChangeSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speedIncrease;
        }
        else if (Input.GetMouseButton(1))
        {
            currentSpeed = speedDeceleration;
        }
        else
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
    private void Dead()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void LoadData()
    {
        StartCoroutine(LoadLevel());
        SaveLoad saveLoad = new SaveLoad();
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        Player player = this;
        saveLoad.LoadData(ref player, ref currentLevelIndex);
        if (currentLevelIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(currentLevelIndex);
        }
    }

    private IEnumerator LoadLevel()
    {
        while (black.color.a > 0)
        {
            yield return new WaitForFixedUpdate();
             Color color = black.color;
             color.a -= 0.01f;
             black.color = color;
        }
    }
    public void EventEndReload()
    {
        anim.SetInteger("Reload", 0);
    }
    public void EventAttackknife()
    {
        float sizeX = 0.8f;
        float sizeY = 1.4f;
        Collider2D[] cols
           = Physics2D.OverlapBoxAll(transform.position + transform.up * sizeX
           , new Vector2(sizeX, sizeY), 0, ~(1 << 8));
        if (cols != null)
        {
            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<AI>().health -= weapon.damageKnife;
                    Effect.Blood(col.transform.position);
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
        GetComponentInChildren<AttackHand>(true)
            .gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    public void EventSoundStep()
    {
        AudioClip soundStep = null;
        switch (UnityEngine.Random.Range(0, 5))
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