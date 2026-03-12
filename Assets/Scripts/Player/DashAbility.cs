using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private float length = 300.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float cost = 50.0f;
    [SerializeField] private StaminaController staminaController;

    private bool isDash;
    private Vector2 direction;
    private float time = 0.0f;

    private void Awake()
    {
        movementSystem = GetComponent<MovementSystem>();

        isDash = false;
        time = 0.0f;

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

    public void Dash(Vector2 dashDirection)
    {
        if (staminaController.TryConsume(cost))
        {
            direction = dashDirection * speed;
            isDash = true;
            time = 0.0f;
        }
    }
}
