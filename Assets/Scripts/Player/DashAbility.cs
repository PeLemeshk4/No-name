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
    private Vector2 direction;
    private float time = 0.0f;

    private void Start()
    {
        movementSystem = GetComponent<MovementSystem>();

        staminaController = GetComponent<StaminaController>();
    }
    
    private void FixedUpdate()
    {
        if (isDash)
        {
            movementSystem.Dash = direction * Time.fixedDeltaTime;
            time += Time.fixedDeltaTime;
            if (time > length / speed)
            {
                movementSystem.Dash = Vector2.zero;
                isDash = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDash)
        {       
            isDash = false;
            movementSystem.Dash = Vector2.zero;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 directionBounce = direction - 2.0f * Vector2.Dot(direction, normal) * normal;
            movementSystem.Bounce(speed * powerOfBounce, directionBounce.normalized);
        }
    }

    public void Dash()
    {
        if (staminaController.TryConsume(cost))
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (cursorPosition - playerPosition).normalized * speed;
            isDash = true;
            time = 0.0f;
        }
    }
}
