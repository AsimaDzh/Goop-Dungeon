using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("========== Moving ==========")]
    [SerializeField] private float movingRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    private Vector2 _movingTarget;
    private float _waitCounter;

    virtual public float MoveSpeed { get; protected set; }

    protected Rigidbody2D _rb;


    private void Start()
    {
        _waitCounter = waitTime;
    }


    private void HandleIdle()
    {
        _rb.linearVelocity = Vector2.zero;

        _waitCounter -= Time.deltaTime;
        if (_waitCounter <= 0f)
        {
            PickNewWalkingPoint();
        }
    }


    private void PickNewWalkingPoint()
    {
        float _randomOffsetX = Random.Range(-movingRadius, movingRadius);
        _movingTarget = new Vector2(
            transform.position.x + _randomOffsetX,
            transform.position.y);
    }


    private void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (direction.x > 0.01f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction.x < -0.01f)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }


    private void HandleMoving()
    {
        float _directionX = Mathf.Sign(_movingTarget.x - transform.position.x);

        _rb.linearVelocity = new Vector2(_directionX * MoveSpeed, 0f);

        Rotate(new Vector2(_directionX, 0f));

        if (Mathf.Abs(_movingTarget.x - transform.position.x) < 0.2f)
        {
            _rb.linearVelocity = Vector2.zero;
            _waitCounter = waitTime;
        }
    }
}
