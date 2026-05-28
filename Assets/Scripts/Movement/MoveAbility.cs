using System;
using UnityEngine;

public class MoveAbility : MonoBehaviour
{
    private MovementSystem movementSystem;
    private TagSpeed tagSpeed;

    private float direction = 0.0f;

    public float Speed
    {
        get
        {
            return tagSpeed.Speed;
        }
    }

    public float Direction 
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init(TagSpeed tagSpeed)
    {
        this.tagSpeed = tagSpeed;

        movementSystem = GetComponent<MovementSystem>();

        enabled = true;
    }

    private void FixedUpdate()
    {
        movementSystem.Move = direction * tagSpeed.Speed;
    }
}
