using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private float move = 0.0f;
    private Vector2 dash = Vector2.zero;
    private bool isDash = false;

    public float Move
    {
        get
        {
            return move;
        }
        set
        {
            move = value;
        }
    }  
    public Vector2 Dash
    {
        get
        {
            return dash;
        }
        set
        {
            dash = value;
            if (dash == Vector2.zero)
            {
                rb.linearVelocity = dash;
                isDash = false;
            }
            else
            {
                isDash = true;
            }    
        }
    }
    public bool IsDash
    {
        get
        {
            return isDash;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void FixedUpdate()
    {
        if (isDash)
        {
            rb.linearVelocity = dash;
        }
        else
        {
            rb.linearVelocityX = move;
        }          
    }

    public void Jump(float power)
    {
        if (!isDash)
        {
            rb.AddForceY(power);
        }
    }
}
