using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private Vector3 launchOffset;
    [SerializeField] private LayerMask hitLayers;
    private Vector3 _startPosition;
    private bool _isThrown;


    public void Setup(float damage, float maxDistance, float speed, LayerMask hitLayers)
    {
        this.damage = damage;
        this.maxDistance = maxDistance;
        this.speed = speed;
        this.hitLayers = hitLayers;
    }


    private void Start()
    {
        if(_isThrown)
        {
            var _direction = -transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(_direction * speed, ForceMode2D.Impulse);
        }
        transform.Translate(launchOffset);
        _startPosition = transform.position;

        Destroy(gameObject, 5f);
    }


    private void Update()
    {
        if (!_isThrown)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }

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
