using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private float length = 300.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float cost = 50.0f;
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
