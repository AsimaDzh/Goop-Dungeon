using UnityEngine;
using Random = UnityEngine.Random;


public class CharacterBase : MonoBehaviour
{
    protected enum CharacterState
    {
        Idle = 0,
        Moving = 1,

        // NPC
        Inspecting = 2,
        Accepting = 3,
        Rejecting = 4,
        Following = 5,

        // Enemy
        Chasing = 6,
        Attacking = 7,
        Dead = 8
    }

    [SerializeField] protected CharacterState _currentState = CharacterState.Idle;

    [Header("========== Moving ==========")]
    [SerializeField] private float movingRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    private Vector2 _movingTarget;
    private float _waitCounter;

    virtual public float MoveSpeed { get; protected set; }

    protected Rigidbody2D _rb;

    [Header("========== Obstacle / Cliff Detection ==========")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform obstaclePoint;
    [SerializeField] private Transform cliffPoint;
    [SerializeField] private float checkDistance = 1f;
    private int _lastBlockedDir = 0;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _waitCounter = waitTime;
    }


    protected void HandleIdle()
    {
        _rb.linearVelocity = Vector2.zero;

        _waitCounter -= Time.deltaTime;
        if (_waitCounter <= 0f)
        {
            PickNewWalkingPoint();
            _currentState = CharacterState.Moving;
        }
    }


    protected void HandleMoving()
    {
        float _directionX = Mathf.Sign(_movingTarget.x - transform.position.x);

        RaycastHit2D _obstacleHit = Physics2D.Raycast(
            obstaclePoint.position,
            transform.right,
            checkDistance,
            obstacleLayer);

        RaycastHit2D _cliffHit = Physics2D.Raycast(
            cliffPoint.position,
            Vector2.down,
            checkDistance,
            groundLayer);

        if (!_obstacleHit.collider || _cliffHit.collider)
        {
            _rb.linearVelocity = Vector2.zero;
            _waitCounter = waitTime;
            _currentState = CharacterState.Idle;
            _lastBlockedDir = (int)Mathf.Sign(_directionX);
            return;
        }

        _rb.linearVelocity = new Vector2(_directionX * MoveSpeed, 0f);

        Rotate(new Vector2(_directionX, 0f));

        if (Mathf.Abs(_movingTarget.x - transform.position.x) < 0.2f)
        {
            _rb.linearVelocity = Vector2.zero;
            _waitCounter = waitTime;
            _currentState = CharacterState.Idle;
        }
    }


    protected void Rotate(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (direction.x > 0.01f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction.x < -0.01f)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }


    private void PickNewWalkingPoint()
    {
        if (_lastBlockedDir != 0)
        {
            int _oppositeDir = -_lastBlockedDir;
            float _randomOffsetX = Random.Range(0.5f, movingRadius) * _oppositeDir;

            _movingTarget = new Vector2(
                transform.position.x + _randomOffsetX,
                transform.position.y);

            _lastBlockedDir = 0;
            return;
        }

        float _randOffsetX = Random.Range(-movingRadius, movingRadius);
       
        _movingTarget = new Vector2(
            transform.position.x + _randOffsetX,
            transform.position.y);
    }
}
