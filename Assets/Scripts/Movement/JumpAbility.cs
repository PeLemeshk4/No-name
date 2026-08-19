using Unity.VisualScripting;
using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private TagJump tagJump;

    private bool onGround = false;
    private float noDoubleJumpTime = 0.1f;
    private float afterJumpTime = 0.0f;
    private bool isJump = false;
    private float coyoteTime = 0.15f;
    private float fallTime = 0.0f;

    public bool NotOnGround { get; private set; } = false;

    public float Power
    {
        get
        {
            return tagJump.Power;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init(TagJump tagJump, Rigidbody2D rb)
    {
        this.tagJump = tagJump;
        this.rb = rb;

        enabled = true;
    }

    private void Update()
    {
        if (afterJumpTime <= noDoubleJumpTime)
        {
            afterJumpTime += Time.deltaTime;
        }
        else
        {
            isJump = false;
        }

        if (NotOnGround && onGround)
        {
            if (fallTime >= coyoteTime)
            {
                onGround = false;
            }
            fallTime += Time.deltaTime;
        }
    }

    public bool Jump(bool afterDash = false)
    {
        if (!onGround) return false;
        if (afterJumpTime < noDoubleJumpTime) return false;

        rb.AddForceY(afterDash ? Power / 4 : Power, ForceMode2D.Impulse);
        afterJumpTime = 0.0f;
        onGround = false;
        NotOnGround = true;
        fallTime = 0.0f;
        isJump = true;
        return true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;
            if (normal.y >= 0.4f)
            {
                onGround = true;
                NotOnGround = false;

                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        NotOnGround = true;
        fallTime = 0.0f;
    }
}
