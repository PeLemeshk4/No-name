using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower = 300.0f;
    private float direction = 0;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void FixedUpdate()
    {
        // ¬цепл€етс€ в стену насмерть, надо что-то делать
        rb.linearVelocityX = direction * speed;
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<float>();
    }

    private void OnJump()
    {
        rb.AddForceY(jumpPower);
    }
}
