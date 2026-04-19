using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask hitLayers;
    private Vector3 _startPosition;


    public void Setup(float damage, float maxDistance, float speed, LayerMask hitLayers)
    {
        this.damage = damage;
        this.maxDistance = maxDistance;
        this.speed = speed;
        this.hitLayers = hitLayers;
    }


    private void Start()
    {
        _startPosition = transform.position;
    }


    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);

        float _traveled = Vector3.Distance(_startPosition, transform.position);
        if (_traveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        Debug.Log($"FireBall hit the {other.name}, with the {damage} damage");

        IDamageable _damageable = other.GetComponent<IDamageable>();
        
        if (_damageable == null)
            _damageable = other.GetComponentInParent<IDamageable>();
        if (_damageable != null)
            _damageable.TakeDamage(damage);

        Destroy(gameObject);
    }
}
