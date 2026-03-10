using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    [SerializeField] private float power = 300.0f;
    [SerializeField] private Rigidbody2D rb;

    private bool onGround;

    private void Awake()
    {
        onGround = false;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        if (onGround)
        {
            rb.AddForceY(power);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y > 0)
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length != 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            if (normal.y == 0)
            {
                onGround = false;
            }
        }
        else
        {
            onGround = false;
        }
    }
}
