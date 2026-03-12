using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private float move;
    private Vector2 dash;
    private bool isDash;

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
            isDash = true;
        }
    }

    private void Awake()
    {
        move = 0.0f;
        dash = Vector2.zero;
        isDash = false;

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDash)
        {
            rb.linearVelocity = dash;
            isDash = false;
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
