using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D _rb2D;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        _rb2D.linearVelocity = new Vector2(_horizontalInput * speed, _rb2D.linearVelocity.y);
        
        //Jump
        if (Input.GetKey(KeyCode.Space))
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, speed);

        //Flip
        if (_horizontalInput > 0.01f)
            transform.localScale = new Vector3(6, 6, 6);
        else if (_horizontalInput < -0.01f)
            transform.localScale = new Vector3(-6, 6, 6);
    }
}
