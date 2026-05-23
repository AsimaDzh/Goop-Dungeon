using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageInterval = 2f;
    private IDamageable _playerDamageable;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _playerDamageable = collision.GetComponent<IDamageable>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerDamageable == null || _playerDamageable.IsDead) return;

        _playerDamageable.TakeDamage(damage);
    }
}
