using UnityEngine;
using System.Collections;


public class FireGoop : EnemyBase
{
    [Header("========== Range Attack ==========")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private LayerMask projectileHitLayers;

    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float flightTime = 1.5f;
    [SerializeField] private float aimOffsetY = 1f;

    [Header("========== Attack Timing ========== ")]
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float recoveryTime = 0.5f;

    private bool _isAttacking;


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

        StartCoroutine(AttackRoutine());
    }


    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        // Wait for attack delay before shooting
        yield return new WaitForSeconds(attackDelay);
        // Shoot the projectile towards the target
        //ShootProjectile();
        // Wait for recovery time after shooting before allowing next attack
        yield return new WaitForSeconds(recoveryTime);
        _isAttacking = false;
    }
}
