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


    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && IsGrounded())
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpForce);
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(
            groundCheck.position,
            new Vector2(0.25f, 0.2f),
            0, groundLayer);
    }
    #endregion
}
