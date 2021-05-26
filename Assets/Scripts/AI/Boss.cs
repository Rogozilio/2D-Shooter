using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStage
{
    Peaceful = 0,
    First = 1,
    Second,
    Third
};
public class Boss : AI
{
    private Transform _spawner1;
    private Transform _spawner2;
    private Transform _spawner3;
    private Transform _spawner4;
    private AudioClip _soundStep;
    private AudioClip _soundAttack2;
    private AudioClip _soundAttack3;
    private AudioClip _soundSpawnMonster;
    private Vector3 _currentDirPush;
    private float _maxHealth;
    private float _currentPushForce;
    private bool _isPuchedBoss;

    public BossStage BossStage;
    [Space]
    [Header("First Stage")]
    public GameObject Light;
    public GameObject Light1;
    public GameObject Light2;
    [Range(5, 20)]
    public float TimeSpawnEnemy;
    public GameObject[] FirstEnemy;

    [Space]
    [Header("Second Stage")]
    public GameObject Light3;
    public GameObject Light4;
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
        _currentPushForce = PushForce;
    }
    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            if (BossStage != BossStage.Third
                && BossStage != BossStage.Peaceful)
            {
                PlaySound(_soundSpawnMonster);
                GameObject enemy1 = Instantiate(FirstEnemy[Random.Range(0, FirstEnemy.Length)]
                    , _spawner1.position, Quaternion.identity); ;
                GameObject enemy2 = Instantiate(FirstEnemy[Random.Range(0, FirstEnemy.Length)]
                    , _spawner2.position, Quaternion.identity);
                enemy1.GetComponentsInChildren<CircleCollider2D>()[1].radius = 100;
                enemy2.GetComponentsInChildren<CircleCollider2D>()[1].radius = 100;
                enemy1.GetComponent<AI>().DistanceDetected = 30;
                enemy2.GetComponent<AI>().DistanceDetected = 30;
            }
            if (BossStage == BossStage.Second)
            {
                GameObject enemy3 = Instantiate(SecondEnemy[Random.Range(0, SecondEnemy.Length)]
                    , _spawner3.position, Quaternion.identity);
                GameObject enemy4 = Instantiate(SecondEnemy[Random.Range(0, SecondEnemy.Length)]
                    , _spawner4.position, Quaternion.identity);
                enemy3.GetComponentsInChildren<CircleCollider2D>()[0].radius = 100;
                enemy4.GetComponentsInChildren<CircleCollider2D>()[0].radius = 100;
                enemy3.GetComponent<AI>().DistanceDetected = 30;
                enemy4.GetComponent<AI>().DistanceDetected = 30;
            }
            yield return new WaitForSeconds(TimeSpawnEnemy);
            if (BossStage == BossStage.Third)
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
            if (BossStage == BossStage.Second)
            {
                animator.SetInteger("Boss", 2);
            }
            if (BossStage == BossStage.Third)
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
        if (agent.remainingDistance <= 1.3)
        {
            _currentDirPush = transform.up;
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
    private void IncludeLamp()
    {
        if(BossStage == BossStage.First)
        {
            Light.GetComponent<Lamp>().ActiveLamp();
            Light1.GetComponent<Lamp>().ActiveLamp();
            Light2.GetComponent<Lamp>().ActiveLamp();
        }
        else if (BossStage == BossStage.Second)
        {
            Light3.GetComponent<Lamp>().ActiveLamp();
            Light4.GetComponent<Lamp>().ActiveLamp();
        }
    }
    private void Update()
    {
        if (BossStage == BossStage.Peaceful)
        {
            if (Vector3.Distance(target.transform.position
            , transform.position) < 7)
            {
                BossStage = BossStage.First;
                IncludeLamp();
                StartCoroutine(SpawnMonster());
            }
            else
                return;
        }
        else if (Health > _maxHealth * (1f / 3f)
            && Health < _maxHealth * (2f / 3f)
            && BossStage != BossStage.Second)
        {
            BossStage = BossStage.Second;
            IncludeLamp();
            animator.SetInteger("Boss", 1);
            StartCoroutine(AttackOnSecondeStage());
        }
        else if (Health <= _maxHealth * (1f / 3f)
            && BossStage != BossStage.Third)
        {
            BossStage = BossStage.Third;
            animator.SetInteger("Boss", 3);
            GetComponentInChildren<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().flipY = false;
            for (int i = 0; i < 5; i++)
                transform.GetChild(0).SetParent(null);
            transform.position
                = new Vector3(transform.position.x, transform.position.y - 0.8f);
        }

        if (BossStage == BossStage.Third)
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
        if (_isPuchedBoss
            && _currentPushForce > 0)
        {
            _currentPushForce -= 0.05f;
            target.GetComponent<Rigidbody2D>()
                .AddRelativeForce(_currentDirPush * Time.fixedDeltaTime * _currentPushForce);
        }
        else
        {
            _currentPushForce = PushForce;
            _isPuchedBoss = false;
        }
    }
}
