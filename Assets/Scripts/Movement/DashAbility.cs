using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    private MovementSystem movementSystem;
    private TagDash tagDash;
    private StaminaController staminaController;

    public bool IsDash { get; private set; } = false;
    private Vector2 dashDirection;
    private float time = 0.0f;

    public float Length
    {
        get
        {
            return tagDash.Length;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init(TagDash tagDash, StaminaController staminaController)
    {
        this.tagDash = tagDash;
        this.staminaController = staminaController;

        movementSystem = GetComponent<MovementSystem>();

        enabled = true;
    }
    
    private void FixedUpdate()
    {
        if (IsDash)
        {
            movementSystem.Dash = dashDirection * tagDash.Speed;
            time += Time.fixedDeltaTime;
            if (time > tagDash.Length / tagDash.Speed)
            {
                movementSystem.Dash = Vector2.zero;
                IsDash = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsDash)
        {
            if (Vector2.Angle(dashDirection, collision.contacts[0].normal) <= 90.0f) return;

            IsDash = false;
            movementSystem.Dash = Vector2.zero;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 directionBounce = dashDirection - 2.0f * Vector2.Dot(dashDirection, normal) * normal;
            movementSystem.Bounce(tagDash.Speed * tagDash.PowerOfBounce, directionBounce.normalized);
        }
    }

    public void Dash(Vector2 direction)
    {
        if (staminaController.TryConsume(tagDash.Cost))
        {
            dashDirection = direction;
            IsDash = true;
            time = 0.0f;
        }
    }
}
