using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    static public GameObject[] bloods;

    private void Awake()
    {
        bloods = GameObject.FindGameObjectsWithTag("Blood");
    }

    static public void CreateBlood(Vector2 pos)
    {
        foreach(GameObject blood in bloods)
        {
            if(blood.GetComponent<Animator>().GetInteger("Blood") == 0)
            {
                Animator anim = blood.GetComponent<Animator>();
                blood.transform.position = new Vector3(pos.x, pos.y, -3);
                anim.Play("Blood" + Random.Range(1, 6).ToString());
                //anim.SetInteger("Blood", Random.Range(1, 6));
                return;
            }
        }
    }
}
