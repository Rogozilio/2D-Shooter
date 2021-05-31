using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    health = 0,
    Shotgun,
    Rifle,
    PistolAmmo,
    ShotgunAmmo,
    RifleAmmo,
}
public class PickUp : MonoBehaviour
{
    private AudioSource _audio;
    public Item Item;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _audio.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(DestroyItem());
        }
    }
    private IEnumerator DestroyItem()
    {
        yield return new WaitWhile(() => _audio.isPlaying);
        Destroy(gameObject);
    }
}
