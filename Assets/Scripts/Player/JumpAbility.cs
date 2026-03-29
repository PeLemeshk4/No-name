using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private float power = 300.0f;

    private bool onGround = false;

    private void Start()
    {
        movementSystem = GetComponent<MovementSystem>();
    }

    public void Jump()
    {
        if (onGround)
        {
            movementSystem.Jump(power);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y >= 0.4f)
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
