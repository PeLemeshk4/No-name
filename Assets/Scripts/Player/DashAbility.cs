using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float power = 300.0f;
    [SerializeField] private float cost = 50.0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private StaminaController staminaController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        staminaController = GetComponent<StaminaController>();
    }

    public void Dash(Vector2 direction)
    {
        if (staminaController.TryConsume(cost))
        {
            rb.AddForce(direction * power);
        }
    }
}
