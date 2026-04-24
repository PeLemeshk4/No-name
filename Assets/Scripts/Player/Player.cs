using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] private MoveAbility moveAbility;
    [SerializeField] private JumpAbility jumpAbility;
    [SerializeField] private TimeSlowAbility timeSlowAbility;
    [SerializeField] private DashAbility dashAbility;
    [SerializeField] private ActiveWeapon activeWeapon;
    [SerializeField] private AttackAbility attackAbility;
    [SerializeField] private ParryAbility parryAbility;

    [SerializeField] private GameObject look;

    private PlayerInput playerInput;
    private Vector2 lookDirection = Vector2.zero;
    private float attackTime = 0.0f;
    private bool isAttacking = false;
    private bool hasSlowing = false;

    private void Awake()
    {   
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Attack"].canceled += OnAttackCanceled;
        playerInput.actions["Attack"].started += OnAttackStarted;

        moveAbility = GetComponent<MoveAbility>();
        jumpAbility = GetComponent<JumpAbility>();
        timeSlowAbility = GetComponent<TimeSlowAbility>();
        dashAbility = GetComponent<DashAbility>();
        activeWeapon = GetComponent<ActiveWeapon>();
        attackAbility = GetComponent<AttackAbility>();
        parryAbility = GetComponent<ParryAbility>();
    }

    private void FixedUpdate()
    {
        Vector2 deltaMouse = Mouse.current.delta.ReadValue();

        if (!(deltaMouse == Vector2.zero || deltaMouse.magnitude <= 5.0f))
        {
            lookDirection = deltaMouse.normalized;
            float newZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            look.transform.eulerAngles = new Vector3(0, 0, newZ);
        }

        if (isAttacking)
        {
            attackTime += Time.fixedDeltaTime;
            if (attackTime > 0.05f)
            {
                activeWeapon.SetWeaponDirection(lookDirection);
                if (!hasSlowing)
                {
                    timeSlowAbility.IsActive = true;
                    hasSlowing = true;
                }
            }
        }
    }

    private void OnMove(InputValue value)
    {
        moveAbility.Direction = value.Get<float>();
    }

    private void OnJump()
    {
        jumpAbility.Jump();
    }

    private void OnDash()
    {
        dashAbility.Dash(lookDirection);
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        isAttacking = true;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (mousePosition.x >= Screen.width / 2)
        {
            activeWeapon.SetWeaponDirection(new Vector2(1, 0));
        }
        else
        {
            activeWeapon.SetWeaponDirection(new Vector2(-1, 0));
        }
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        attackAbility.Attack(activeWeapon.Weapon);
        isAttacking = false;
        attackTime = 0.0f;
        hasSlowing = false;
        timeSlowAbility.IsActive = false;
    }
}
