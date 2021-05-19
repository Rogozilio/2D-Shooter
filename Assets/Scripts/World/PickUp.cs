using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _audio.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(DestroyItem());
        }
    }
    private IEnumerator DestroyItem()
    {
        yield return new WaitWhile(() => _audio.isPlaying);
        Destroy(gameObject);
    }
}