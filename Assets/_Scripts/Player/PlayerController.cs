using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour, IPlayerController
{
    [Header("========== References ==========")]
    [SerializeField] private GoopData goopData;
    private GrabObjects _grabSystem;

    private Rigidbody2D _rb2D;
    private BoxCollider2D _boxCollider;

    private FrameInput _frameInput;
    private Vector2 _frameVelocity;

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    private float _time;
    private float _timeJumpWasPressed;
    private float _frameLeftGrounded = float.MinValue;

    private bool _cachedQueryStartInCol;
    private bool _grounded;
    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;

    private bool HasBufferedJump =>
        _bufferedJumpUsable &&
        _time < _timeJumpWasPressed + goopData.JumpBuffer;

    private bool CanUseCoyote =>
        _coyoteUsable && !_grounded &&
        _time < _frameLeftGrounded + goopData.CoyoteTime;


    private void Awake()
    {
        _grabSystem = GetComponent<GrabObjects>();
        _rb2D = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _cachedQueryStartInCol = Physics2D.queriesStartInColliders;
    }


    private void Update()
    {
        _time += Time.deltaTime;
    }


    private void FixedUpdate()
    {
        CheckCollisions();
        HandleJump();
        HandleDirection();
        HandleGravity();

        _rb2D.linearVelocity = _frameVelocity;
    }


    #region PLAYER_CONTROLS

    public void ОnThrow(InputAction.CallbackContext context)
    {
        if (context.started && _grabSystem._isObjectGrabbed)
        {
            _grabSystem.ThrowObject();
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _frameInput.Move = context.ReadValue<Vector2>();

        if (_frameInput.Move.x > 0.01f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (_frameInput.Move.x < -0.01f)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        // Turns smooth input into hard one
        if (goopData.SnapInput)
        {
            if (Mathf.Abs(_frameInput.Move.x) < goopData.HorizontalDeadZoneThreshold)
                _frameInput.Move.x = 0;
            else _frameInput.Move.x = Mathf.Sign(_frameInput.Move.x);
        }
    }


    // Smoothly accelerates and slows down player compared to different sensations on the ground and in the air.
    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var _decel = _grounded ? goopData.GroundDeceleration : goopData.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(
                _frameVelocity.x, 0, 
                _decel * Time.deltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(
                _frameVelocity.x,
                _frameInput.Move.x * goopData.MaxSpeed,
                goopData.Acceleration * Time.fixedDeltaTime);
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _frameInput.JumpDown = true;
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }

        if (context.canceled) _frameInput.JumpHeld = false;

        if (context.performed) _frameInput.JumpHeld = true;
    }


    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb2D.linearVelocity.y > 0)
            _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote)
        {
            ExecuteJump();
            _jumpToConsume = false;
            _frameInput.JumpDown = false;
        }
    }


    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = goopData.JumpPower;
        Jumped?.Invoke();
    }
    #endregion


    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        bool _groundHit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.size,
            0, Vector2.down,
            goopData.GrounderDistance,
            ~goopData.PlayerLayer);

        bool _ceilingHit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.size,
            0, Vector2.up,
            goopData.GrounderDistance,
            ~goopData.PlayerLayer);

        if (_ceilingHit) 
            _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        if(!_grounded && _groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        else if (_grounded && !_groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInCol;
    }


    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0)
            _frameVelocity.y = goopData.GroundingForce;
        else
        {
            float _gravity = goopData.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0)
                _gravity *= goopData.JumpEndEarlyGravMod;

            _frameVelocity.y = Mathf.MoveTowards(
                _frameVelocity.y,
                -goopData.MaxFallSpeed,
                _gravity * Time.fixedDeltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (goopData == null) return;
        if (_boxCollider == null) return;

        // Center and size of the box collider
        Vector3 center = _boxCollider.bounds.center;
        Vector3 size3 = new Vector3(
            _boxCollider.size.x, 
            _boxCollider.size.y, 
            0.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size3);

        // Bottom point of the box collider
        Vector3 bottom = center - new Vector3(
            0f, _boxCollider.size.y * 0.5f, 0f);

        // Draw the line representing the ground check distance
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(bottom, bottom + Vector3.down * goopData.GrounderDistance);

        // Draw a thin box at the end of the ground check line to visualize the area being checked for ground
        Gizmos.DrawWireCube(
            bottom + Vector3.down * goopData.GrounderDistance, 
            new Vector3(_boxCollider.size.x, 0.02f, 0.02f));

        // Do a box cast to check for ground and change color based on whether it hits something or not
        RaycastHit2D hit = Physics2D.BoxCast(
            (Vector2)center, 
            _boxCollider.size, 0f, 
            Vector2.down, 
            goopData.GrounderDistance, 
            ~goopData.PlayerLayer);
        Gizmos.color = hit.collider != null ? Color.green : Color.red;

        // If it hits something, draw a wire cube at the hit point and a line from the bottom of the box collider to the hit point
        if (hit.collider != null)
        {
            Vector3 hitPoint = (Vector3)hit.centroid;
            Gizmos.DrawWireCube(hitPoint, size3);
            Gizmos.DrawLine(bottom, hitPoint);
        }
    }
#endif
}


public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
}


public interface IPlayerController
{
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;
    public Vector2 FrameInput { get; }
}
