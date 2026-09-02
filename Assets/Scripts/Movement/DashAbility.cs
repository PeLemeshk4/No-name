using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private TagDash tagDash;
    private StaminaController staminaController;

    private Vector2 dashDirection = Vector2.zero;
    private float realLength = 0.0f;
    private float dashDistance = 0.0f;
    private Vector2 dashFallVelocity = Vector2.zero;
    private float dashFallAccelerate = Physics.gravity.y;

    private float bounceXPower = 0.0f;
    private float bounceTime = 0.0f;
    private float xResistance = 0.0f;

    public bool IsDash { get; private set; } = false;
    public Vector2 DashVelocity { get; private set; } = Vector2.zero;

    public bool IsBounce { get; private set; } = false;
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
    public float MaxSlowing
    {
        get
        {
            return tagDash.MaxSlowing;
        }
    }
    public float BouncePower
    {
        get
        {
            return tagDash.BouncePower;
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
            dashDistance += ((DashVelocity - dashFallVelocity) * Time.fixedDeltaTime).magnitude;
            if (dashDistance > realLength)
            {
                IsDash = false;
                DashVelocity = Vector2.zero;
                rb.linearVelocityY = rb.linearVelocityY >= 0.0f ? 0.0f : rb.linearVelocityY;
            }
            else if (dashDistance >= realLength * 2.0f / 3.0f)
            {
                dashFallVelocity.y += dashFallAccelerate * Time.fixedDeltaTime;
                DashVelocity = dashDirection * Speed * (1 - GetDashSlowing()) + dashFallVelocity;
            }
            else
            {
                DashVelocity = dashDirection * Speed * (1 - GetDashSlowing());
            }       
        }
        else if (IsBounce)
        {
            xResistance = bounceTime * bounceTime * 2;
            if (xResistance >= Mathf.Abs(bounceXPower))
            {
                IsBounce = false;
                BounceXVelocity = 0.0f;

                return;
            }
            BounceXVelocity = (Mathf.Abs(bounceXPower) - xResistance) * (bounceXPower / Mathf.Abs(bounceXPower));
            bounceTime += Time.fixedDeltaTime;
        }
    }

    private float GetDashSlowing()
    {
        if (dashDistance >= realLength) return MaxSlowing;

        float delta = Mathf.Pow(0.2f, Distribute(0, realLength, 0, 3, realLength - dashDistance));
        float resistance = delta;

        return resistance > MaxSlowing ? MaxSlowing : resistance;
    }

    private float Distribute(float min, float max, float newMin, float newMax, float value)
    {
        float percent = value / (max - min);
        float newValue = percent * (newMax - newMin) + newMin;

        return newValue;
    }


    private Vector2 bounceDirection = Vector2.zero;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsDash)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(DashVelocity, contact.normal) < 100.0f) continue;

                Vector2 normal = contact.normal;
                bounceDirection = DashVelocity - 2.0f * Vector2.Dot(DashVelocity, normal) * normal;
                float power = Mathf.Abs(DashVelocity.magnitude) * BouncePower;
                IsDash = false;
                DashVelocity = Vector2.zero;
                rb.linearVelocityY = 0.0f;
                Bounce(power, bounceDirection.normalized);

                return;
            }
        }
        else if (IsBounce)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(bounceDirection, contact.normal) < 80.0f) continue;

                IsBounce = false;
                BounceXVelocity = 0.0f;

                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDash)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(DashVelocity, contact.normal) < 100.0f) continue;

                Vector2 normal = contact.normal;
                bounceDirection = DashVelocity - 2.0f * Vector2.Dot(DashVelocity, normal) * normal;
                float power = Mathf.Abs(DashVelocity.magnitude) * BouncePower;
                IsDash = false;
                DashVelocity = Vector2.zero;
                rb.linearVelocityY = 0.0f;
                Bounce(power, bounceDirection.normalized);

                return;
            }
        }
        else if (IsBounce)
        {
            IsBounce = false;
            BounceXVelocity = 0.0f;
        }
    }

    public void Dash(Vector2 direction, float dashPower)
    {
        if (staminaController.TryConsume(tagDash.Cost))
        {
            IsBounce = false;
            BounceXVelocity = 0.0f;

            realLength = Length * dashPower;
            dashDirection = direction * dashPower;
            DashVelocity = dashDirection * Speed;
            IsDash = true;
            dashDistance = 0.0f;
            dashFallVelocity = Vector2.zero;
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
