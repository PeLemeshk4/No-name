using System;
using UnityEngine;

public class PlayerVelocityManager : MonoBehaviour 
{
    private Rigidbody2D rb;

    private Func<float> move;
    private Func<Vector2> dash;
    private Func<float> bounceX;
    private Func<bool> notOnGround;

    private void Awake()
    {
        enabled = false;
    }
    public void Init(Rigidbody2D rb, Func<float> moveVelocity, Func<Vector2> dashVelocity, Func<float> bounceXVelocity, Func<bool> notOnGround)
    {
        this.rb = rb;
        move = moveVelocity;
        dash = dashVelocity;
        bounceX = bounceXVelocity;
        this.notOnGround = notOnGround;

        enabled = true;
    }

    private void FixedUpdate()
    {
        if (dash() == Vector2.zero)
        {
            rb.linearVelocityX = move() + bounceX();
            
        }
        else rb.linearVelocity = dash();
    }
}
