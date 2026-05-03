using UnityEngine;
using System;


public class EnemyBase : CharacterBase, IDamageable
{
    [Header("========== Enemy Data ==========")]
    [SerializeField] private EnemyData enemyData;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    [Header("========== Target ==========")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoResolveTargetOnStart = true; // if true, will try to find the player by tag on Start()

    [Header("========== Attack ==========")]
    [SerializeField] private float attackCooldown = 1f;
    private float _nextAttackTime;

    [Header("========== Death ==========")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelayAfterDeath = 0.15f;
    private bool _isDead;
    private IDamageable _damageable; 

    public EnemyData Data => enemyData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => enemyData != null ? enemyData.MaxHealth : 0f;
    override public float MoveSpeed => enemyData != null ? enemyData.MoveSpeed : 0f;
    public float Damage => enemyData != null ? enemyData.Damage : 0f;
    public float AttackRange => enemyData != null ? enemyData.AttackRange : 0f;
    public float DetectionWidth => enemyData != null ? enemyData.DetectionWidth : 0f;
    public float DetectionHeight => enemyData != null ? enemyData.DetectionHeight : 0f;
    public bool IsDead => _isDead;
    protected Transform CurrentTarget => target;

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
                _currentState = CharacterState.Attacking;
            else 
                _currentState = CharacterState.Chasing;
        }
        else if (_currentState == CharacterState.Chasing || _currentState == CharacterState.Attacking)
            _currentState = CharacterState.Idle;


        switch (_currentState)
        {
            case CharacterState.Idle:
                HandleIdle();
                break;

            case CharacterState.Moving:
                HandleMoving();
                break;

            case CharacterState.Chasing:
                MoveTowardsTarget();
                break;

            case CharacterState.Attacking:
                TryAttack();
                break;
        }
    }


    public void MoveTowardsTarget()
    {
        if (target == null) return;

        float _directionX = Mathf.Sign(target.position.x - transform.position.x);
        
        _rb.linearVelocity = new Vector2(_directionX * MoveSpeed, 0f);

        Rotate(new Vector2(_directionX, 0f));
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
        _currentState = CharacterState.Dead;
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
        _currentState = CharacterState.Idle;
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
