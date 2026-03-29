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
    [SerializeField] private AttackAbility attackAbility;
    [SerializeField] private Weapon weapon;

    private PlayerInput playerInput;

    private void Start()
    {   
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Slowing"].canceled += OnSlowingCanceled;

        moveAbility = GetComponent<MoveAbility>();
        jumpAbility = GetComponent<JumpAbility>();
        staminaController = GetComponent<StaminaController>();
        timeSlowAbility = GetComponent<TimeSlowAbility>();
        dashAbility = GetComponent<DashAbility>();
        attackAbility = GetComponent<AttackAbility>();
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
        dashAbility.Dash();
    }

    private void OnAttack()
    {
        attackAbility.Attack();
    }

    private void OnParry()
    {
        timeSlowAbility.TryParry();
    }
}
