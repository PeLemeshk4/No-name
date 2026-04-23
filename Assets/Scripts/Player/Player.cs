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

    private void Awake()
    {   
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Slowing"].canceled += OnSlowingCanceled;

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

        if (deltaMouse == Vector2.zero || deltaMouse.magnitude <= 5.0f) return;

        lookDirection = deltaMouse.normalized;
        float newZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
        look.transform.eulerAngles = new Vector3(0, 0, newZ);

        if (activeWeapon != null) 
            activeWeapon.SetWeaponDirection(lookDirection);
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
        dashAbility.Dash(lookDirection);
    }

    private void OnAttack()
    {
        activeWeapon.SetWeaponDirection(lookDirection);
        attackAbility.Attack(activeWeapon.Weapon);
    }

    private void OnParry()
    {
        activeWeapon.SetWeaponDirection(lookDirection);
        parryAbility.Parry(activeWeapon.Weapon);
    }
}
