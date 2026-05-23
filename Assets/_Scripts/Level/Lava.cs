using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageInterval = 2f;
    private IDamageable _playerDamageable;
    private float _nextDamageTime;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _playerDamageable = collision.GetComponent<IDamageable>();
        _nextDamageTime = damageInterval;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerDamageable == null || _playerDamageable.IsDead) return;

        _nextDamageTime += Time.deltaTime;
        if (_nextDamageTime >= damageInterval)
        {
            _playerDamageable.TakeDamage(damage);
            _nextDamageTime = 0f;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        _playerDamageable = null;
        _nextDamageTime = 0f;
    }
}
