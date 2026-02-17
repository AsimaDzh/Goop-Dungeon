using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private ScriptableStats stats;

    private Rigidbody2D _rb2D;
    private BoxCollider2D _boxCollider;

    private FrameInput _frameInput;
    private Vector2 _frameVelocity;

    private bool _cachedQueryStartInCol;

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    private float _time;

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump =>
        _bufferedJumpUsable &&
        _time < _timeJumpWasPressed + stats.JumpBuffer;

    private bool CanUseCoyote =>
        _coyoteUsable && !_grounded &&
        _time < _frameLeftGrounded + stats.CoyoteTime;


    private void Awake()
    {
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
        ApplyMovement();
    }


    #region PLAYER_CONTROLS
    public void OnMove(InputAction.CallbackContext context)
    {
        _frameInput = context.ReadValue<Vector2>().x;
        
        if (_frameInput > 0.01f)
            transform.localScale = new Vector3(6, 6, 6);
        else if (_frameInput < -0.01f)
            transform.localScale = new Vector3(-6, 6, 6);

        // Turns smooth input into hard one
        if (stats.SnapInput)
        {
            if (Mathf.Abs(_frameInput.Move.x) < stats.HorizontalDeadZoneThreshold)
                _frameInput.Move.x = 0;
            else _frameInput.Move.x = Mathf.Sign(_frameInput.Move.x);
        }
    }


    // Smoothly accelerates and slows down player compared to different sensations on the ground and in the air.
    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var _decel = _grounded ? stats.GroundDeceleration : stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(
                _frameVelocity.x, 0, 
                _decel * Time.deltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(
                _frameVelocity.x,
                _frameInput.Move.x * stats.MaxSpeed,
                stats.Acceleration * Time.fixedDeltaTime);
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

        if (_grounded || CanUseCoyote) ExecuteJump();

        _jumpToConsume = false;
        _frameInput.JumpDown = false;
    }


    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = stats.JumpPower;
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
            stats.GrounderDistance,
            ~stats.PlayerLayer);

        bool _ceilingHit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.size,
            0, Vector2.up,
            stats.GrounderDistance,
            ~stats.PlayerLayer);

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
            _frameVelocity.y = stats.GroundingForce;
        else
        {
            if (_endedJumpEarly && _frameVelocity.y > 0)
                stats.FallAcceleration *= stats.JumpEndEarlyGravMod;

            _frameVelocity.y = Mathf.MoveTowards(
                _frameVelocity.y,
                -stats.MaxFallSpeed,
                stats.FallAcceleration * Time.fixedDeltaTime);
        }
    }
}
