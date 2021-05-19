using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStage
{
    First = 1,
    Second,
    Third
};
public class Boss : AI
{
    
    private BossStage _bossStage;
    private Transform _spawner1;
    private Transform _spawner2;
    private Transform _spawner3;
    private Transform _spawner4;
    private AudioClip _soundStep;
    private AudioClip _soundAttack2;
    private AudioClip _soundAttack3;
    private AudioClip _soundSpawnMonster;
    private float     _maxHealth;
    private float     _currentPushForce;
    private bool      _isPuchedBoss;

    [Space]
    [Header("First Stage")]
    [Range(5, 20)]
    public float TimeSpawnEnemy;
    public GameObject[] FirstEnemy;

    [Space]
    [Header("Second Stage")]
    public GameObject[] SecondEnemy;
    public GameObject Projectile;
    [Range(1, 10)]
    public float SpeedProjectile;
    [Range(1, 10)]
    public float SpeedRotateProjectile;

    [Space]
    [Header("Third Stage")]
    public float PushForce;

    private void OnEnable()
    {
        _spawner1 = GetComponentsInChildren<Transform>()[2];
        _spawner2 = GetComponentsInChildren<Transform>()[3];
        _spawner3 = GetComponentsInChildren<Transform>()[4];
        _spawner4 = GetComponentsInChildren<Transform>()[5];
        _soundStep = Resources.Load<AudioClip>("Sounds/AI/Boss/soundStep");
        _soundAttack2 = Resources.Load<AudioClip>("Sounds/AI/Boss/soundAttack2");
        _soundAttack3 = Resources.Load<AudioClip>("Sounds/AI/Boss/soundAttack3");
        _soundSpawnMonster = Resources.Load<AudioClip>("Sounds/AI/Boss/soundSpawnMonster");
        soundDead = Resources.Load<AudioClip>("Sounds/AI/Boss/soundDead");


        _maxHealth = Health;
        _bossStage = BossStage.First;
        _currentPushForce = PushForce;
        StartCoroutine(SpawnMonster());
    }
    private IEnumerator SpawnMonster()
    {
        while(true)
        {
            if(_bossStage != BossStage.Third)
            {
                PlaySound(_soundSpawnMonster);
                Instantiate(FirstEnemy[Random.Range(0, FirstEnemy.Length)]
                    , _spawner1.position, Quaternion.identity); ;
                Instantiate(FirstEnemy[Random.Range(0, FirstEnemy.Length)]
                    , _spawner2.position, Quaternion.identity);
            }
            if(_bossStage == BossStage.Second)
            {
                Instantiate(SecondEnemy[Random.Range(0, SecondEnemy.Length)]
                    , _spawner3.position, Quaternion.identity);
                Instantiate(SecondEnemy[Random.Range(0, SecondEnemy.Length)]
                    , _spawner4.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(TimeSpawnEnemy);
            if(_bossStage == BossStage.Third)
            {
                yield break;
            }
        }
    }
    private IEnumerator AttackOnSecondeStage()
    {
        while (true)
        {
            animator.SetInteger("Boss", 1);
            yield return new WaitForSeconds(0.5f);
            if(_bossStage == BossStage.Second)
            {
                animator.SetInteger("Boss", 2);
            }
            if (_bossStage == BossStage.Third)
                yield break;
            yield return new WaitForSeconds(3f);
        }
    }
    private void EventShotProjectileSecondStage()
    {
        float radius = 1f;
        Vector3 startPosProjectile = transform.position
            + Vector3.ClampMagnitude(transform.up, radius);
        GameObject bullet = Instantiate(Projectile, startPosProjectile, transform.rotation);
        bullet.GetComponent<BossProjectile>().Speed = SpeedProjectile;
        bullet.GetComponent<BossProjectile>().SpeedRotate = SpeedRotateProjectile;
        bullet.GetComponent<BossProjectile>().Damage = Damage;
        bullet.GetComponent<BossProjectile>().Target = target;
        PlaySound(_soundAttack2);
    }
    private void Attack()
    {
        if (agent.remainingDistance <= 0.8)
        {
            animator.SetInteger("Enemy", 2);
        }
    }
    private void EventAttackThirdStage()
    {
        _isPuchedBoss = true;
        target.GetComponent<Player>().Health -= Damage;
        PlaySound(_soundAttack3, false, true);
    }
    
    private void EventSoundStep()
    {
        PlaySound(_soundStep);
    }
    private void Dead()
    {
        if (Health <= 0)
        {
            StopAllCoroutines();
            audio.Stop();
            PlaySound(soundDead, false, true);
            if (!audio.isPlaying)
                audio.enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            agent.enabled = false;
            enabled = false;
            animator.Play("DeadBoss");
        }
    }
    private void Update()
    {
        if(Health >= _maxHealth*(2f / 3f) 
            && _bossStage != BossStage.First)
        {
            _bossStage = BossStage.First;
        }
        else if(Health > _maxHealth * (1f / 3f) 
            && Health < _maxHealth * (2f / 3f)
            && _bossStage != BossStage.Second)
        {
            _bossStage = BossStage.Second;
            animator.SetInteger("Boss", 1);
            StartCoroutine(AttackOnSecondeStage());
        }
        else if(Health <= _maxHealth * (1f / 3f)
            && _bossStage != BossStage.Third)
        {
            _bossStage = BossStage.Third;
            animator.SetInteger("Boss", 3);
            GetComponentInChildren<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().flipY = false;
            for(int i = 0; i < 5; i++)
                transform.GetChild(0).SetParent(null);
            transform.position 
                = new Vector3(transform.position.x, transform.position.y - 0.8f);
        }

        if(_bossStage == BossStage.Third)
        {
            AnimationMove();
            Attack();
            RotateAtTarget();
            agent.SetDestination(target.transform.position);
            Dead();
        }
    }
    private void FixedUpdate()
    {
        if(_isPuchedBoss
            && _currentPushForce > 0)
        {
            _currentPushForce -= 0.05f;
            target.GetComponent<Rigidbody2D>()
                .AddRelativeForce(transform.up * Time.fixedDeltaTime * _currentPushForce);
        }
        else
        {
            _currentPushForce = PushForce;
            _isPuchedBoss = false;
        }
    }
}
