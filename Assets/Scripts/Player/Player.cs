using System;
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

    [SerializeField] private CircleTimer circleTimer;
    [SerializeField] private GameObject look;
    [SerializeField] private float timeOfAiming = 2.0f;

    private PlayerInput playerInput;
    private Vector2 lookDirection = Vector2.zero;
    private bool isAttacking = false;
    private bool isDashing = false;
    private float time = 0.0f;
    private bool hasSlowing = false;

    private void Awake()
    {   
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Dash"].started += OnDashStarted;
        playerInput.actions["Dash"].canceled += OnDashCanceled;
        playerInput.actions["Attack"].canceled += OnAttackCanceled;
        playerInput.actions["Attack"].started += OnAttackStarted;     

        moveAbility = GetComponent<MoveAbility>();
        jumpAbility = GetComponent<JumpAbility>();
        timeSlowAbility = GetComponent<TimeSlowAbility>();
        dashAbility = GetComponent<DashAbility>();
        activeWeapon = GetComponent<ActiveWeapon>();
        attackAbility = GetComponent<AttackAbility>();

        look.SetActive(false);

        circleTimer.timerEnded += TimerEnd;
    }

    private void Update()
    {
        Vector2 deltaMouse = Mouse.current.delta.ReadValue();

        if (!(deltaMouse == Vector2.zero || deltaMouse.magnitude <= 5.0f))
        {
            lookDirection = deltaMouse.normalized;
            float newZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            look.transform.eulerAngles = new Vector3(0, 0, newZ);
        }

        if (isDashing)
        {
            time += Time.deltaTime;
            if (!hasSlowing)
            {
                SlowTimeStart();
            }
        }
        else if (isAttacking)
        {
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                activeWeapon.SetWeaponDirection(lookDirection);
                if (!hasSlowing)
                {
                    SlowTimeStart();
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

    private void OnDashStarted(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        isDashing = true;
    }

    private void OnDashCanceled(InputAction.CallbackContext context)
    {
        if (!isDashing) return;

        dashAbility.Dash(lookDirection);
        circleTimer.StopTimer();
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (isDashing) return;

        isAttacking = true;
        activeWeapon.SetWeaponDirection(new Vector2(lookDirection.x, 0).normalized);
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (!isAttacking) return;

        attackAbility.Attack(activeWeapon.Weapon);
        circleTimer.StopTimer();   
    }

    private void SlowTimeStart()
    {
        timeSlowAbility.IsActive = true;
        hasSlowing = true;

        look.SetActive(true);
        circleTimer.StartTimer(timeOfAiming);
    }

    private void TimerEnd(object o, EventArgs e)
    {
        look.SetActive(false);

        timeSlowAbility.IsActive = false;
        hasSlowing = false;
        time = 0.0f;

        if (isAttacking) isAttacking = false;
        else if (isDashing) isDashing = false;
    }

}
