using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shot : MonoBehaviour
{
    public GameObject ammoP;
    public GameObject ammoR;
    public GameObject ammoS;
    public Transform shotDir;

    private Animator anim;

    private float timeShot;
    public float startTimeP;
    public float startTimeR;
    public float startTimeS;

    public int currentAmmoP;
    public int AllAmmoP;
    public int fullAmmoP;

    public int currentAmmoR;
    public int AllAmmoR;
    public int fullAmmoR;

    public int currentAmmoS;
    public int AllAmmoS;
    public int fullAmmoS;

    public bool rezP, rezR, rezS;
    public bool Rifle = false;
    public bool Shotgun = false;

    public GameObject soundP;
    public GameObject soundR;
    public GameObject soundS;
    public GameObject ReloadPi;
    public GameObject ReloadRi;
    public GameObject ReloadSu;
    public GameObject Up;

    [SerializeField]
    private Text ammoCount;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rezP = true;
            rezR = false;
            rezS = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && Rifle == true)
        {
            rezP = false;
            rezR = true;
            rezS = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && Shotgun == true)
        {
            rezP = false;
            rezR = false;
            rezS = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rezP = false;
            rezR = false;
            rezS = false;
        }

        if (timeShot <= 0)
        {
            if (Input.GetMouseButtonDown(0) && currentAmmoP > 0)
            {
                if (rezP == true)
                {
                    Instantiate(ammoP, shotDir.position, transform.rotation);
                    Destroy(Instantiate(soundP, shotDir.position, Quaternion.identity),1f);
                    
                    timeShot = startTimeP;
                    currentAmmoP -= 1;
                }
            }
            if (Input.GetMouseButton(0) && currentAmmoR > 0)
            {
                if (rezR == true)
                {
                    Instantiate(ammoR, shotDir.position, transform.rotation);
                    timeShot = startTimeR;
                    Destroy(Instantiate(soundR, shotDir.position, Quaternion.identity), 0.2f);
                    currentAmmoR -= 1;
                }
            }
            if (Input.GetMouseButtonDown(0) && currentAmmoS > 0)
            {
                if (rezS == true)
                {
                    Instantiate(ammoS, shotDir.position, transform.rotation);
                    timeShot = startTimeS;
                    Destroy(Instantiate(soundS, shotDir.position, Quaternion.identity), 1f);
                    currentAmmoS -= 2;
                }
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
        
        if (rezP == true)
        {
            ammoCount.text = currentAmmoP + " / " + AllAmmoP;
        }
        else if (rezR == true)
        {
            ammoCount.text = currentAmmoR + " / " + AllAmmoR;
        }
        else if (rezS == true)
        {
            ammoCount.text = currentAmmoS + " / " + AllAmmoS;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ammoCount.text = " ";
        }
        if (Input.GetKey(KeyCode.R) && AllAmmoP > 0 && currentAmmoP != 10 && rezP == true)
        {
            anim.SetBool("ReloadP", true);
            
            Invoke("ReloadP", 1.1f);
            

        }
        if (Input.GetKey(KeyCode.R) && AllAmmoR > 0 && currentAmmoR != 30 && rezR == true)
        {

            Invoke("ReloadR", 1.3f);
            anim.SetBool("ReloadR", true);


        }
        if (Input.GetKey(KeyCode.R) && AllAmmoS > 0 && currentAmmoS != 6 && rezS == true)
        {

            Invoke("ReloadS", 1.35f);
            anim.SetBool("ReloadS", true);


        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rifle>())
        {
            Rifle = true;
            Destroy(Instantiate(Up, shotDir.position, Quaternion.identity), 0.2f);
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<Shotgun>())
        {
            Shotgun = true;
            Destroy(Instantiate(Up, shotDir.position, Quaternion.identity), 0.2f);
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<PistolClip>())
        {
            AllAmmoP += 10;
            Destroy(Instantiate(Up, shotDir.position, Quaternion.identity), 0.2f);
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<RifleClip>())
        {
            AllAmmoR += 20;
            Destroy(Instantiate(Up, shotDir.position, Quaternion.identity), 0.2f);
            Destroy(collision.gameObject);
        }
        if (collision.GetComponent<ShotgunClip>())
        {
            AllAmmoS += 15;
            Destroy(Instantiate(Up, shotDir.position, Quaternion.identity), 0.2f);
            Destroy(collision.gameObject);
        }


    }

    public void ReloadP()
    {
        
        int reason = 10 - currentAmmoP;
        if (AllAmmoP >= reason)
        {
            AllAmmoP = AllAmmoP - reason;
            currentAmmoP = 10;
        }
        else
        {
            currentAmmoP = currentAmmoP + AllAmmoP;
            AllAmmoP = 0;
            
        }
            anim.SetBool("ReloadP", false);

    }
    public void EventReloadP()
    {
        Destroy(Instantiate(ReloadPi, shotDir.position, Quaternion.identity), 1.1f);
    }
    public void EventReloadR()
    {
        Destroy(Instantiate(ReloadRi, shotDir.position, Quaternion.identity), 1.3f);
    }
    public void EventReloadS()
    {
        Destroy(Instantiate(ReloadSu, shotDir.position, Quaternion.identity), 1.35f);
    }
    public void ReloadR()
    {
        int reason = 30 - currentAmmoR;
        if (AllAmmoR >= reason)
        {
            AllAmmoR = AllAmmoR - reason;
            currentAmmoR = 30;
        }
        else
        {
            currentAmmoR = currentAmmoR + AllAmmoR;
            AllAmmoR = 0;

        }

            anim.SetBool("ReloadR", false);
    }
    public void ReloadS()
    {
        int reason = 6 - currentAmmoS;
        if (AllAmmoS >= reason)
        {
            AllAmmoS = AllAmmoS - reason;
            currentAmmoS = 6;
        }
        else
        {
            currentAmmoS = currentAmmoS + AllAmmoS;
            AllAmmoS = 0;

        }
            anim.SetBool("ReloadS", false);
    }
}
