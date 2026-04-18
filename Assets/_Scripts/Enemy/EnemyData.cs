using UnityEngine;

[CreateAssetMenu( fileName = "EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType
    {
        Melee,   // Goblins
        Ranged,  // Red slimes that shoot projectiles
        Boss
    }

    [Header("========== General ==========")]
    public string EnemyName = "New Enemy";
    public EnemyType Type = EnemyType.Melee;

    [Header("========== Characteristics ==========")]
    [Min(1f)] public float MaxHealth = 50f;
    [Min(0f)] public float MoveSpeed = 3f;
    [Min(0f)] public float Damage = 10f;

    [Header("========== Attack ==========")]
    [Min(0f)] public float AttackRange = 2f;
    [Min(0f)] public float DetectionWidth = 10f;
    [Min(0f)] public float DetectionHeight = 4f;

    [Header("========== Reward ==========")]
    [Min(0f)] public float ExperienceReward = 10f;

    [Header("========== Prefab ==========")]
    public GameObject Prefab;
}
