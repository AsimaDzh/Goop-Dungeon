using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("========== Player Settings ==========")]
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
    }
    #endregion

    //private void Update()
    //{
    //    float _horizontalInput = Input.GetAxis("Horizontal");
    //    _rb2D.linearVelocity = new Vector2(
    //        _horizontalInput * speed, 
    //        _rb2D.linearVelocity.y);

    //    //Jump
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        _rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

    //    //Flip
    //    if (_horizontalInput > 0.01f)
    //        transform.localScale = new Vector3(6, 6, 6);
    //    else if (_horizontalInput < -0.01f)
    //        transform.localScale = new Vector3(-6, 6, 6);
    //}
}
