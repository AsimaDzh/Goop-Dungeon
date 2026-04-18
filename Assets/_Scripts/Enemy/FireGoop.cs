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


    public override void Attack()
    {
        if (CurrentTarget == null) return;

        if (projectilePrefab == null)
        {
            Debug.LogWarning($"{name}: projectilePrefab is not assigned.", this);
            return;
        }

        if (projectileHitLayers.value == 0)
            Debug.LogWarning($"{name}: projectileHitLayers is empty.", this);

        // Projectile shoot logic
    }
}
