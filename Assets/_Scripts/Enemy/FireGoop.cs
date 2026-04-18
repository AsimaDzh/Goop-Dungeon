using UnityEngine;

public class FireGoop : EnemyBase
{
    [Header("========== Range Attack ==========")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private LayerMask projectileHitLayers;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float projectileDistance = 12f;
    [SerializeField] private float aimOffsetY = 1f;
}
