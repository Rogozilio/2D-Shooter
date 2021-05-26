using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    private Animator _animator;
    private Light2D _light;
    [SerializeField]
    private bool _isActiveLight = false;
    private float _baseOuterRadius;

    public bool StartBlinkLight = false;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _light = GetComponent<Light2D>();
        _baseOuterRadius = _light.pointLightOuterRadius;
        if (_isActiveLight)
            ActiveLamp();
        else
            _light.enabled = false;
        if(StartBlinkLight)
            StartCoroutine(BlinkLight());
    }
    private IEnumerator BlinkLight()
    {
        _light.enabled = true;
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.2f));
            if (_animator.GetInteger("Lamp") == 2)
                yield break;
            _light.pointLightOuterRadius = Random.Range(0.5f, 1.5f);
        }
    }
    public void ActiveLamp()
    {
        if(!_isActiveLight)
        {
            _light.enabled = true;
            _isActiveLight = true;
            _animator.SetInteger("Lamp", 2);
            _light.pointLightOuterRadius = _baseOuterRadius;
            GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isActiveLight
            && other.CompareTag("Player"))
            _animator.SetInteger("Lamp", 1);    
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_isActiveLight
            && other.CompareTag("Player"))
            _animator.SetInteger("Lamp", 0);
    }
}
