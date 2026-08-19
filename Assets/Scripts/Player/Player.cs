using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;

    private MoveAbility moveAbility;
    private JumpAbility jumpAbility;
    private TimeSlowAbility timeSlowAbility;
    private DashAbility dashAbility;
    private ActiveWeapon activeWeapon;
    private AttackAbility attackAbility;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private CircleTimer circleTimer;
    [SerializeField] private GameObject look;

    private StateManager sM;
    private AnimationManager aM;

    // Parameters
    private const float quickActionTime = 0.05f;
    private const float aimingTime = 2.0f;
    private const float bufferTime = 0.2f;

    // Variables
    private float moveDirection = 0;
    private float characterDirection = 1;
    private Vector2 lookDirection = Vector2.zero;
    private bool startAiming = false;
    private bool aiming = false;
    private float aimmingTime = 0.0f;
    private bool wantJump = false;
    private float currentBufferTime = 0;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sM = new StateManager(States.Idle);
        aM = new AnimationManager(animator, sM);

        circleTimer.timerEnded += TimerEnd;

        sM.AddBlockedState(States.Dash, () => dashAbility.IsDash);
        sM.AddBlockedState(States.Attack, () => activeWeapon.Weapon.IsAttacking);        

        enabled = true;
    }

    private void Update()
    {
        SetLookDirection();

        if (sM.IsStateLocked())
        {
            moveAbility.Direction = 0;
            return;
        }

        // Определение состояния игрока
        if (!jumpAbility.NotOnGround)
        {
            if (Mathf.Abs(rb.linearVelocityX) > 0.1) sM.SetState(States.Run);
            else sM.SetState(States.Idle);
        }
        else if (rb.linearVelocityY > 0) sM.SetState(States.Jump);
        else if (rb.linearVelocityY < 0) sM.SetState(States.Fall);

        moveAbility.Direction = moveDirection;
        characterDirection = moveDirection != 0 ? moveDirection : characterDirection;

        // Буферизация прыжка
        if (wantJump)
        {
            if (currentBufferTime <= bufferTime)
            {
                if (jumpAbility.Jump(dashAbility.IsBounce))
                {
                    wantJump = false;
                }
                else
                {
                    currentBufferTime += Time.deltaTime;
                }   
            }
            else
            {
                wantJump = false;
            }
        }

        // Поворот спрайта от направления
        if (aiming)
        {
            if (Mathf.Abs(lookDirection.x) > 0)
            {
                sr.flipX = lookDirection.x < 0;
            }
        }
        else
        {
            sr.flipX = characterDirection < 0;
        }

        // Логика начала прицеливания
        if (!aiming && startAiming)
        {
            if (aimmingTime < quickActionTime)
            {
                aimmingTime += Time.deltaTime;
            }
            else
            {
                StartAiming();
            }
        }    
    }

    private void SetLookDirection()
    {
        Vector2 deltaMouse = Mouse.current.delta.ReadValue();
        if (!(deltaMouse == Vector2.zero || deltaMouse.magnitude <= 5.0f))
        {
            lookDirection = deltaMouse.normalized;
            float newZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            look.transform.eulerAngles = new Vector3(0, 0, newZ);
        }
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<float>();
    }

    private void OnJump()
    {
        wantJump = true;
        currentBufferTime = 0.0f;
    }

    private void OnDashStarted(InputAction.CallbackContext context)
    {
        if (startAiming) return;
        startAiming = true;
        aimmingTime = 0.0f;
    }

    private void OnDashCanceled(InputAction.CallbackContext context)
    {
        if (!startAiming) return;

        if (!sM.IsStateLocked())
        {
            dashAbility.Dash(lookDirection);
            sM.SetState(States.Dash);
        }

        startAiming = false;

        if (!aiming) return;
        circleTimer.StopTimer();
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (startAiming) return;
        startAiming = true;
        aimmingTime = 0.0f;
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (!startAiming) return;

        if (!sM.IsStateLocked())
        {
            if (aiming)
            {
                attackAbility.Attack(activeWeapon.Weapon, lookDirection);
            }
            else
            {
                attackAbility.Attack(activeWeapon.Weapon, new Vector2(characterDirection, 0));
            }
            sM.SetState(States.Attack);
        }

        startAiming = false;

        if (!aiming) return;
        circleTimer.StopTimer();
    }

    private void StartAiming()
    {
        aiming = true;
        timeSlowAbility.IsActive = true;

        look.SetActive(true);
        circleTimer.StartTimer(aimingTime);
    }

    private void TimerEnd(object o, EventArgs e)
    {
        startAiming = false;
        aiming = false;
        timeSlowAbility.IsActive = false;

        look.SetActive(false);    
    }
}
