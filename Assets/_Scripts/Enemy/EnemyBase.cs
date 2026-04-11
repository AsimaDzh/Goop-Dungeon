using UnityEngine;
using System;

public class EnemyBase : MonoBehaviour
{
    private enum EnemyState
    {
        Chase,
        Attack,
        Dead
    }

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

    private EnemyState _currentState = EnemyState.Chase;

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
}
