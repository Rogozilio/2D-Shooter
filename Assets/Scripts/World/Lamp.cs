using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private Animator _animator;
    private Light _light;
    [SerializeField]
    private bool _isActiveLight = false;
    private float _baseSpotAngel;

    public bool StartBlinkLight = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _light = GetComponentInChildren<Light>();
        _baseSpotAngel = _light.spotAngle;
        _light.spotAngle = 0f;
        if(StartBlinkLight)
            StartCoroutine(BlinkLight());
    }
    private IEnumerator BlinkLight()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1f));
            if (_animator.GetInteger("Lamp") == 2)
                yield break;
            _light.spotAngle = 15f;
            yield return new WaitForSeconds(Random.Range(0f, 0.2f));
            if (_animator.GetInteger("Lamp") == 2)
                yield break;
            _light.spotAngle = Random.Range(20f, 25f);
        }
    }
    public void ActiveLamp()
    {
        if(!_isActiveLight)
        {
            _isActiveLight = true;
            _animator.SetInteger("Lamp", 2);
            _light.spotAngle = _baseSpotAngel;
            GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isActiveLight)
            _animator.SetInteger("Lamp", 1);    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isActiveLight)
            _animator.SetInteger("Lamp", 0);
    }
}
