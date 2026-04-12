using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("========== Links ==========")]
    [SerializeField] private EnemyBase enemyBase;

    public EnemyData EnemyData => enemyBase != null ? enemyBase.Data : null;
    public float MaxHealth => enemyBase != null ? enemyBase.MaxHealth : 0f;
    public float MoveSpeed => enemyBase != null ? enemyBase.MoveSpeed : 0f;
    public float Damage => enemyBase != null ? enemyBase.Damage : 0f;
    public float AttackRange => enemyBase != null ? enemyBase.AttackRange : 0f;
    public float DetectionRange => enemyBase != null ? enemyBase.DetectionRange : 0f;
    public float ExperienceReward => EnemyData != null ? EnemyData.ExperienceReward : 0f;

    // Death event in EnemyStats format for backward compatibility.
    public event Action<EnemyStats> OnDied;
}
