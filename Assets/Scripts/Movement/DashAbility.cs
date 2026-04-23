using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private float length = 300.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float cost = 50.0f;
    [SerializeField] private float powerOfBounce = 1.0f;
    [SerializeField] private StaminaController staminaController;

    private bool isDash = false;
    private Vector2 dashDirection;
    private float time = 0.0f;

    private void Awake()
    {
        movementSystem = GetComponent<MovementSystem>();

        staminaController = GetComponent<StaminaController>();
    }
    
    private void FixedUpdate()
    {
        if (isDash)
        {
            movementSystem.Dash = dashDirection * Time.fixedDeltaTime;
            time += Time.fixedDeltaTime;
            if (time > length / speed)
            {
                movementSystem.Dash = Vector2.zero;
                isDash = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDash)
        {
            if (Vector2.Angle(dashDirection, collision.contacts[0].normal) <= 90.0f) return;

            isDash = false;
            movementSystem.Dash = Vector2.zero;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 directionBounce = dashDirection - 2.0f * Vector2.Dot(dashDirection, normal) * normal;
            movementSystem.Bounce(speed * powerOfBounce, directionBounce.normalized);
        }
    }

    public void Dash(Vector2 direction)
    {
        if (staminaController.TryConsume(cost))
        {
            dashDirection = direction * speed;
            isDash = true;
            time = 0.0f;
        }
    }
}
