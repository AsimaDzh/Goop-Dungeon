using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;


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

        if (_isAttacking) return;

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

        yield return new WaitForSeconds(attackDelay);

        ShootProjectile();

        yield return new WaitForSeconds(recoveryTime);
        _isAttacking = false;
    }


    private void ShootProjectile()
    {
        if (projectilePrefab == null || CurrentTarget == null) return;

        Vector2 _targetPos = (Vector2)CurrentTarget.position + Vector2.up * aimOffsetY;

        GameObject _gameObject = Instantiate(
            projectilePrefab,
            shootOrigin.position,
            Quaternion.identity);

        FireBall _fireball = _gameObject.GetComponent<FireBall>();
        if (_fireball == null)
        {
            Debug.LogWarning($"{name}: No fireball");
            Destroy(_gameObject);
            return;
        }

        _fireball.Setup(projectileDamage, projectileHitLayers);

        // Вычисление начальной скорости для попадания в цель за flightTime с учётом гравитации:
        // s = v*t + 0.5*g*t^2  =>  v = (s - 0.5*g*t^2)/t
        //Vector2 gravity = Physics2D.gravity * projRb.gravityScale;
        //Vector2 displacement = targetPos - origin;
        //Vector2 initialVelocity = (displacement - 0.5f * gravity * flightTime * flightTime) / flightTime;

        //fb.Launch(initialVelocity);
    }
}
