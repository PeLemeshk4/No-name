using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] private MoveAbility moveAbility;
    [SerializeField] private JumpAbility jumpAbility;
    [SerializeField] private StaminaController staminaController;
    [SerializeField] private TimeSlowAbility timeSlowAbility;
    [SerializeField] private DashAbility dashAbility;

    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Slowing"].canceled += OnSlowingCanceled;

        moveAbility = GetComponent<MoveAbility>();
        jumpAbility = GetComponent<JumpAbility>();
        staminaController = GetComponent<StaminaController>();
        timeSlowAbility = GetComponent<TimeSlowAbility>();
        dashAbility = GetComponent<DashAbility>();
    }

    private void OnMove(InputValue value)
    {
        moveAbility.Direction = value.Get<float>();
    }

    private void OnJump()
    {
        jumpAbility.Jump();
    }

    private void OnSlowing()
    {
        timeSlowAbility.IsActive = true;
    }

    private void OnSlowingCanceled(InputAction.CallbackContext context)
    {
        timeSlowAbility.IsActive = false;
    }

    private void OnDash()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (cursorPosition - playerPosition).normalized;
        dashAbility.Dash(direction);
    }

    private void OnAttack()
    {

    }
}
