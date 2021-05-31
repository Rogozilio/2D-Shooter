using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private float       _speed;
    private float       _speedRotate;
    private float       _damage;
    private Vector3     _dirTarget;
    private GameObject  _target;
    private Rigidbody2D _rigidBody;
    [HideInInspector]
    public float Speed { set => _speed = value; }
    [HideInInspector]
    public float SpeedRotate { set => _speedRotate = value; }
    [HideInInspector]
    public float Damage { set => _damage = value; }
    [HideInInspector]
    public GameObject Target { set => _target = value; }
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 270f);
        StartCoroutine(ProjectileExtinction());
    }
    private IEnumerator ProjectileExtinction()
    {
        while(true)
        {
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
            transform.localScale -= new Vector3(0.1f, 0.1f);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void Update()
    {
        _dirTarget = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(_dirTarget.y, _dirTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speedRotate);
        _rigidBody.velocity = transform.right * _speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Health -= _damage;
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<AI>().health -= _damage * 0.2f;
        }
        if (!other.CompareTag("Projectile")
            && other.name != ("DetectionZone")
            && other.name != ("Boss"))
        {
            Destroy(gameObject);
        }
    }
}