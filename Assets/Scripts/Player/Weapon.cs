using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public enum EquipWeapon
{
    Knife = 1,
    Pistol,
    Shotgun,
    Rifle
}
public class Weapon : MonoBehaviour
{
    private AudioSource audio;
    private AudioClip soundAttackKnife;
    private AudioClip soundShotPistol;
    private AudioClip soundShotShotgun;
    private AudioClip soundShotRifle;
    private AudioClip soundReload;

    private GameObject bullet;
    public Transform shotDir;

    private float timeShot;
    public float startTimeP;
    public float startTimeR;
    public float startTimeS;

    public float damageKnife;

    public int currentAmmoP;
    public int allAmmoP;

    public int currentAmmoR;
    public int allAmmoR;

    [SerializeField]
    private int bulletsPerShot;
    public int currentAmmoS;
    public int allAmmoS;

    public EquipWeapon inHand;

    public Image ammoImage;
    public Text ammoCount;
    private float StartTime
    {
        get
        {
            if (inHand == EquipWeapon.Pistol)
                return startTimeP;
            if (inHand == EquipWeapon.Shotgun)
                return startTimeS;
            else
                return startTimeR;
        }
    }
    private AudioClip SoundShot
    {
        get
        {
            if (inHand == EquipWeapon.Pistol)
                return soundShotPistol;
            if (inHand == EquipWeapon.Shotgun)
                return soundShotShotgun;
            else
                return soundShotRifle;
        }
    }
    private bool LeftMouseButton
    {
        get
        {
            if (inHand == EquipWeapon.Rifle)
                return Input.GetMouseButton(0);
            else
                return Input.GetMouseButtonDown(0);
        }
    }
    private int BulletsPerShot
    {
        get
        {
            if (inHand == EquipWeapon.Shotgun)
                return bulletsPerShot;
            else
                return 1;
        }
    }
    public int AllAmmo
    {
        get
        {
            if (inHand == EquipWeapon.Pistol)
                return allAmmoP;
            if (inHand == EquipWeapon.Shotgun)
                return allAmmoS;
            if (inHand == EquipWeapon.Rifle)
                return allAmmoR;
            else
                return 0;
        }
        set
        {
            if (inHand == EquipWeapon.Pistol)
                allAmmoP = value;
            if (inHand == EquipWeapon.Shotgun)
                allAmmoS = value;
            if (inHand == EquipWeapon.Rifle)
                allAmmoR = value;
            else
                return;
        }
    }
    public int CurrentAmmo
    {
        get
        {
            if (inHand == EquipWeapon.Pistol)
                return currentAmmoP;
            if (inHand == EquipWeapon.Shotgun)
                return currentAmmoS;
            if (inHand == EquipWeapon.Rifle)
                return currentAmmoR;
            else
                return 0;
        }
        set
        {
            if (inHand == EquipWeapon.Pistol)
                currentAmmoP = value;
            if (inHand == EquipWeapon.Shotgun)
                currentAmmoS = value;
            if (inHand == EquipWeapon.Rifle)
                currentAmmoR = value;
            else
                return;
        }
    }

    void Awake()
    {
        audio = GetComponent<AudioSource>();

        bullet = Resources.Load<GameObject>("Prefabs/Player/Bullet");
        soundAttackKnife = Resources.Load<AudioClip>("Sounds/Player/soundAttackKnife");
        soundShotPistol = Resources.Load<AudioClip>("Sounds/Player/soundShotPistol");
        soundShotShotgun = Resources.Load<AudioClip>("Sounds/Player/soundShotShotgun");
        soundShotRifle = Resources.Load<AudioClip>("Sounds/Player/soundShotRifle");
        soundReload = Resources.Load<AudioClip>("Sounds/Player/soundReload");
    }
    public void Shot(bool isReadyShot, float angle)
    {
        if (timeShot <= 0)
        {
            if (LeftMouseButton
                && CurrentAmmo > 0
                && isReadyShot)
            {
                for (int i = BulletsPerShot; i != 0; i--)
                {
                    Instantiate(bullet, shotDir.position + Vector3.forward
                            , transform.rotation * Quaternion.AngleAxis(UnityEngine.Random.Range(-angle, angle)
                            , transform.forward));
                }
                audio.PlayOneShot(SoundShot);
                timeShot = StartTime;
                CurrentAmmo -= 1;
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
    }
    public EquipWeapon Reload(EquipWeapon currentWeapon)
    {
        int clip = 0;
        switch (currentWeapon)
        {
            case EquipWeapon.Pistol:
            clip = 10;
            break;
            case EquipWeapon.Shotgun:
            clip = 6;
            break;
            case EquipWeapon.Rifle:
            clip = 30;
            break;
        }
        if (clip != CurrentAmmo)
        {
            if (AllAmmo > clip - CurrentAmmo)
            {
                AllAmmo -= clip - CurrentAmmo;
                CurrentAmmo = clip;
            }
            else
            {
                CurrentAmmo += AllAmmo;
                AllAmmo = 0;
            }
        }
        return currentWeapon;
    }
    public void EventSoundReload()
    {
        audio.PlayOneShot(soundReload);
    }
    public void EventSoundAttack()
    {
        audio.PlayOneShot(soundAttackKnife);
    }
}