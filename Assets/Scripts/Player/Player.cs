using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower = 300.0f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRecovery = 10f;
    [SerializeField] private float slowingFactor = 0.3f;
    [SerializeField] private float slowingCost = 50f;
    private float direction = 0;
    private float stamina;

    public Interface playerInterface;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Slowing"].canceled += OnSlowingCanceled;
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<float>();
    }

    private bool onGround = false;
    private void OnJump()
    {
        if (onGround)
        {
            rb.AddForceY(jumpPower);
            onGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y > 0)
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length != 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            if (normal.y == 0)
            {
                onGround = false;
            }
        }
        else
        {
            onGround = false;
        }
    }

    private bool isSlowing = false;
    private void OnSlowing()
    {
        if (stamina > maxStamina * 0.2f || isSlowing)
        {
            isSlowing = true;
            Time.timeScale = slowingFactor;
        }
    }

    private void OnSlowingCanceled(InputAction.CallbackContext context)
    {
        Time.timeScale = 1f;
        isSlowing = false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = direction * speed;

        if (isSlowing)
        {
            stamina -= Time.fixedDeltaTime / Time.timeScale * slowingCost;
            if (stamina <= 0)
            {
                Time.timeScale = 1f;
                isSlowing = false;
            }
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += Time.fixedDeltaTime * staminaRecovery;
                if (stamina > maxStamina)
                {
                    stamina = maxStamina;
                }
            }
        }
        playerInterface.staminaS.value =
            stamina / maxStamina * playerInterface.staminaS.maxValue;
    }

}
