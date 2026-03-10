using UnityEngine;

public class MoveAbility : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Rigidbody2D rb;

    private float direction;
    private bool isUpdateDirection;

    public float Direction 
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            isUpdateDirection = true;
        }
    }

    private void Awake()
    {
        direction = 0.0f;
        isUpdateDirection = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isUpdateDirection)
        {
            rb.linearVelocityX = direction * speed;
            isUpdateDirection = false;
        }
    }
}
