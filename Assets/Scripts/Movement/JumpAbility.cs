using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    private MovementSystem movementSystem;
    private TagJump tagJump;

    public bool OnGround { get; private set; } = false;

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
    public void Init(TagJump tagJump)
    {
        this.tagJump = tagJump;

        movementSystem = GetComponent<MovementSystem>();

        enabled = true;
    }

    public bool Jump()
    {
        if (!OnGround) return false;

        movementSystem.Jump(tagJump.Power);
        return true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y >= 0.4f)
        {
            OnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length != 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            if (normal.y == 0)
            {
                OnGround = false;
            }
        }
        else
        {
            OnGround = false;
        }
    }
}
