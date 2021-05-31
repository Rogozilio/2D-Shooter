using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private static GameObject[] effects;

    private void Awake()
    {
        effects = GameObject.FindGameObjectsWithTag("Blood");
    }

    public static void Blood(Vector2 pos)
    {
        foreach(GameObject effect in effects)
        {
            if(effect.GetComponent<Animator>().GetInteger("Effect") == 0)
            {
                Animator anim = effect.GetComponent<Animator>();
                effect.transform.position = new Vector3(pos.x, pos.y, -3);
                anim.Play("Blood" + Random.Range(1, 6).ToString());
                return;
            }
        }
    }
    public static void Projectile(Vector2 pos)
    {
        foreach(GameObject effect in effects)
        {
            if(effect.GetComponent<Animator>().GetInteger("Effect") == 0)
            {
                Animator anim = effect.GetComponent<Animator>();
                effect.transform.position = new Vector3(pos.x, pos.y, -3);
                anim.Play("Projectile");
                return;
            }
        }
    }
}
