using UnityEngine;
using System;
using Random = UnityEngine.Random;

enum EnemyState
{
    Idle = 0,
    Patrolling = 1,
    Chasing = 2,
    Attacking = 3,
    Dead = 4
}


public class EnemyBase : CharacterBase, IDamageable
{
    [Header("========== Enemy Data ==========")]
    [SerializeField] private EnemyData enemyData;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;
    private EnemyState _currentState = EnemyState.Idle;

    [Header("========== Target ==========")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoResolveTargetOnStart = true; // if true, will try to find the player by tag on Start()

    [Header("========== Attack ==========")]
    [SerializeField] private float attackCooldown = 1f;
    private float _nextAttackTime;

    [Header("========== Patrol ==========")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    private Vector2 _patrolTarget;
    private float _waitCounter;

    [Header("========== Death ==========")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelayAfterDeath = 0.15f;
    private bool _isDead;
    private IDamageable _damageable; 

    public EnemyData Data => enemyData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => enemyData != null ? enemyData.MaxHealth : 0f;
    public float MoveSpeed => enemyData != null ? enemyData.MoveSpeed : 0f;
    public float Damage => enemyData != null ? enemyData.Damage : 0f;
    public float AttackRange => enemyData != null ? enemyData.AttackRange : 0f;
    public float DetectionWidth => enemyData != null ? enemyData.DetectionWidth : 0f;
    public float DetectionHeight => enemyData != null ? enemyData.DetectionHeight : 0f;
    public bool IsDead => _isDead;
    protected Transform CurrentTarget => target;
    protected Rigidbody2D _rb;

    public event Action OnDied;


    private void Awake()
    {
        currentHealth = enemyData != null ? enemyData.MaxHealth : 0f;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        if (target == null && autoResolveTargetOnStart)
            ResolveTargetOnce();

        if (target != null)
            _damageable = target.GetComponent<IDamageable>();

        _waitCounter = waitTime;
    }


    private void Update()
    {
        if (enemyData == null || _isDead || target == null)
            return;

        float _distance = Vector2.Distance(transform.position, target.position);

        bool _isTargetInDetectionRange =
            Mathf.Abs(target.position.x - transform.position.x) <= DetectionWidth / 2 &&
            Math.Abs(target.position.y - transform.position.y) <= DetectionHeight / 2; 

        if (_isTargetInDetectionRange)
        {
            if (_distance <= AttackRange)
                _currentState = EnemyState.Attacking;
            else 
                _currentState = EnemyState.Chasing;
        }
        else if (_currentState == EnemyState.Chasing || _currentState == EnemyState.Attacking)
            _currentState = EnemyState.Idle;


        switch (_currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Patrolling:
                HandlePatrol();
                break;

            case EnemyState.Chasing:
                MoveTowardsTarget();
                break;

            case EnemyState.Attacking:
                TryAttack();
                break;
        }
    }


    private void HandleIdle()
    {
        _rb.linearVelocity = Vector2.zero;

        _waitCounter -= Time.deltaTime;
        if (_waitCounter <= 0f)
        {
            PickNewPatrolPoint();
            _currentState = EnemyState.Patrolling;
        }
    }


    private void HandlePatrol()
    {
        float _directionX = Mathf.Sign(_patrolTarget.x - transform.position.x);
        
        _rb.linearVelocity = new Vector2(_directionX * MoveSpeed, 0f);
        
        Rotate(new Vector2(_directionX, 0f));

        if (Mathf.Abs(_patrolTarget.x - transform.position.x) < 0.2f)
        {
            _rb.linearVelocity = Vector2.zero;
            _waitCounter = waitTime;
            _currentState = EnemyState.Idle;
        }
    }


    private void PickNewPatrolPoint()
    {
        float _randomOffsetX = Random.Range(-patrolRadius, patrolRadius);
        _patrolTarget = new Vector2(
            transform.position.x + _randomOffsetX, 
            transform.position.y);
    }


    public void MoveTowardsTarget()
    {
        if (target == null) return;

        float _directionX = Mathf.Sign(target.position.x - transform.position.x);
        
        _rb.linearVelocity = new Vector2(_directionX * MoveSpeed, 0f);

        Rotate(new Vector2(_directionX, 0f));
    }


    private void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (direction.x > 0.01f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction.x < -0.01f)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }


    public virtual void Attack()
    {
        if (target == null) return;

        if (_damageable != null && !_damageable.IsDead)
            _damageable.TakeDamage(Damage);

        Debug.Log($"{name} attack {target.name} with {Damage} damage");
    }


    private void TryAttack()
    {
        if (Time.time < _nextAttackTime) return;

        _rb.linearVelocity = Vector2.zero;

        _nextAttackTime = Time.time + attackCooldown;

        Attack();
    }


    public virtual void TakeDamage(float damage)
    {
        if (enemyData == null || damage <= 0f || _isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);
        Debug.Log($"{name}: got {damage} damage. Health: {currentHealth}/{MaxHealth}");

        if (currentHealth <= 0f) Die();
    }


    public virtual void Die()
    {
        if (_isDead) return;

        _isDead = true;
        _currentState = EnemyState.Dead;
        Debug.Log($"{name} died :)");
        OnDied?.Invoke();

        bool _isPooledEnemy = GetComponent("PooledEnemy") != null;

        if (!destroyOnDeath || _isPooledEnemy) return;
        if (destroyDelayAfterDeath > 0f)
            Destroy(gameObject, destroyDelayAfterDeath);
        else Destroy(gameObject);
    }


    public void Setup(EnemyData data)
    {
        enemyData = data;
        currentHealth = enemyData.MaxHealth;
        _isDead = false;
        _nextAttackTime = 0f;  
        _currentState = EnemyState.Idle;
    }


    private void ResolveTargetOnce()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) target = player.transform;
    }


    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        Gizmos.color = Color.yellow;
        Vector3 _detectionSize = new Vector3(DetectionWidth, DetectionHeight, 0);
        Gizmos.DrawWireCube(transform.position, _detectionSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
