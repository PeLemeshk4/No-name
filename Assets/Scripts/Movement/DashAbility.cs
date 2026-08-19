using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private TagDash tagDash;
    private StaminaController staminaController;

    private Vector2 dashDirection = Vector2.zero;
    private float dashTime = 0.0f;
    private float bounceXPower = 0.0f;
    private float bounceTime = 0.0f;
    private float xResistance = 0.0f;

    private bool isDash = false;
    public bool IsDash
    {
        get
        {
            return isDash;
        }
        private set
        {
            isDash = value;
            if (!value)
            {
                rb.linearVelocityY = 0.0f;
                DashVelocity = Vector2.zero;
            }
        }
    }
    public Vector2 DashVelocity { get; private set; } = Vector2.zero;
    private bool isBounce = false;
    public bool IsBounce
    {
        get
        {
            return isBounce;
        }
        private set
        {
            isBounce = value;
            if (!value)
            {
                BounceXVelocity = 0.0f;
            }
        }
    }
    public float BounceXVelocity { get; private set; } = 0.0f;

    public float Length
    {
        get
        {
            return tagDash.Length;
        }
    }

    public float Speed
    {
        get
        {
            return tagDash.Speed;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init(TagDash tagDash, StaminaController staminaController, Rigidbody2D rb)
    {
        this.tagDash = tagDash;
        this.staminaController = staminaController;
        this.rb = rb;

        enabled = true;
    }
    
    private void FixedUpdate()
    {
        if (IsDash)
        {
            if (dashTime > Length / Speed)
            {
                IsDash = false;

                return;
            }
            DashVelocity = dashDirection * tagDash.Speed;
            dashTime += Time.fixedDeltaTime;
        }
        else if (IsBounce)
        {
            xResistance = bounceTime * bounceTime * 2;
            if (xResistance >= Mathf.Abs(bounceXPower))
            {
                IsBounce = false;

                return;
            }
            BounceXVelocity = (Mathf.Abs(bounceXPower) - xResistance) * (bounceXPower / Mathf.Abs(bounceXPower));
            bounceTime += Time.fixedDeltaTime;
        }
    }

    private Vector2 bounceDirection = Vector2.zero;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsDash)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(dashDirection, contact.normal) < 100.0f) continue;

                IsDash = false;
                Vector2 normal = contact.normal;
                bounceDirection = dashDirection - 2.0f * Vector2.Dot(dashDirection, normal) * normal;
                Bounce(Speed * tagDash.PowerOfBounce, bounceDirection.normalized);

                return;
            }
        }
        else if (isBounce)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(bounceDirection, contact.normal) < 80.0f) continue;

                IsBounce = false;

                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBounce)
        {
            IsBounce = false;
        }
    }

    public void Dash(Vector2 direction)
    {
        if (staminaController.TryConsume(tagDash.Cost))
        {
            IsBounce = false;
            dashDirection = direction;
            IsDash = true;
            dashTime = 0.0f;
        }
    }

    public void Bounce(float power, Vector2 direction)
    {
        rb.AddForceY(direction.y * power, ForceMode2D.Impulse);
        bounceXPower = direction.x * power;
        IsBounce = true;
        bounceTime = 0.0f;
    }
}
