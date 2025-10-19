using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb2D;


    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2D.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.linearVelocity.y);
        
        if (Input.GetKey(KeyCode.Space))
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, speed);
    }
}
