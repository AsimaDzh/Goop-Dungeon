using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject hitEffect;

    private Rigidbody2D _rb2D;


    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }


    public void Setup(float damage, LayerMask hitLayers)
    {
        this.damage = damage;
        this.hitLayers = hitLayers;
    }


    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    private void FixedUpdate()
    {
        if (_rb2D.linearVelocity.sqrMagnitude > 0.001f)
        {
            float _angle = Mathf.Atan2(_rb2D.linearVelocity.y, _rb2D.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, _angle + 90f);
        }
    }


    public void Launch(Vector2 velocity)
    {
        _rb2D.linearVelocity = velocity;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        Debug.Log($"FireBall hit the {other.name}, with the {damage} damage");

        IDamageable _damageable = other.GetComponent<IDamageable>();
        
        if (_damageable == null)
            _damageable = other.GetComponentInParent<IDamageable>();
        if (_damageable != null)
            _damageable.TakeDamage(damage);

        Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
