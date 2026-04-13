using UnityEngine;
using System;

enum EnemyState
{
    Idle = 0,
    Patrolling = 1,
    Chasing = 2,
    Attacking = 3,
    Dead = 4
}


public class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("========== Enemy Data ==========")]
    [SerializeField] private EnemyData enemyData;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    [Header("========== Stats ==========")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoResolveTargetOnStart = true; // if true, will try to find the player by tag on Start()
    
    [SerializeField] private float attackCooldown = 1f;
    private float _nextAttackTime;

    [Header("========== Death ==========")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelayAfterDeath = 0.15f;
    private bool _isDead;
    private IDamageable _damageable;

    private EnemyState _currentState = EnemyState.Chasing;

    public EnemyData Data => enemyData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => enemyData != null ? enemyData.MaxHealth : 0f;
    public float MoveSpeed => enemyData != null ? enemyData.MoveSpeed : 0f;
    public float Damage => enemyData != null ? enemyData.Damage : 0f;
    public float AttackRange => enemyData != null ? enemyData.AttackRange : 0f;
    public float DetectionRange => enemyData != null ? enemyData.DetectionRange : 0f;
    public bool IsDead => _isDead;
    protected Transform CurrentTarget => target;

    public event Action OnDied;


    private void Awake()
    {
        currentHealth = enemyData != null ? enemyData.MaxHealth : 0f;
    }


    private void Start()
    {
        if (target == null && autoResolveTargetOnStart)
            ResolveTargetOnce();

        _damageable = GetComponent<IDamageable>();
    }


    private void Update()
    {
        if (enemyData == null || _isDead || target == null)
            return;
        
        _damageable ??= target.GetComponent<IDamageable>(); // if _damageable == null
        
        if (_damageable != null && _damageable.IsDead)
            return;

        float _distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        if (_distanceToTarget > DetectionRange) return;
        if (_distanceToTarget <= AttackRange) 
            _currentState = EnemyState.Attacking;
        else _currentState = EnemyState.Chasing;

        switch (_currentState)
        {
            case EnemyState.Attacking:
                TryAttack();
                break;
            case EnemyState.Chasing:
                MoveTowardsTarget();
                break;
        }
    }


    public void Setup(EnemyData data)
    {
        enemyData = data;
        currentHealth = enemyData.MaxHealth;
        _isDead = false;
        _nextAttackTime = 0f;
        _currentState = EnemyState.Chasing;
    }


    public virtual void TakeDamage(float damage)
    {
        if (enemyData == null || damage <= 0f || _isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);
        Debug.Log($"{name}: got {damage} damage. Health: {currentHealth}/{MaxHealth}");

        if (currentHealth <= 0f)
            Die();
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


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    private void ResolveTargetOnce()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) target = player.transform;
    }


    public void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector3 _direction = (target.position - transform.position).normalized;
        _direction.y = 0f;

        transform.position += _direction * MoveSpeed * Time.deltaTime;

        if (_direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                lookRotation, 
                Time.deltaTime * 5f);
        }
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

        _nextAttackTime = Time.time + attackCooldown;
        
        Attack();
    }


    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
