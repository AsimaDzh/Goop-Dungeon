using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("========== Player Settings ==========")]
    [SerializeField] private ScriptableStats stats;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("========== Grounding ==========")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D _rb2D;
    private float _horizontal;


    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rb2D.linearVelocity = new Vector2(
            _horizontal * speed,
            _rb2D.linearVelocity.y);
    }


    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;
        if (_horizontal > 0.01f)
            transform.localScale = new Vector3(6, 6, 6);
        else if (_horizontal < -0.01f)
            transform.localScale = new Vector3(-6, 6, 6);
    }


    public void Jump(InputAction.CallbackContext context)
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
