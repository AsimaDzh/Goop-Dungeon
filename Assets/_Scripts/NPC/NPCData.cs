using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Data/NPC Data")]
public class NPCData : ScriptableObject
{
    public enum NPCType
    {
        Melee,
        Ranged
    }

    [Header("========== General ==========")]
    public string EnemyName = "Unknown Goop";
    public NPCType Type = NPCType.Melee;
    public string Likes = ItemsNames.Rock;

    [Header("========== Characteristics ==========")]
    [Min(1f)] public float MaxHealth = 100f;
    [Min(0f)] public float MoveSpeed = 3f;

    [Header("========== Attack ==========")]
    [Min(0f)] public float Damage = 10f;
    [Min(0f)] public float AttackRange = 2f;
}
