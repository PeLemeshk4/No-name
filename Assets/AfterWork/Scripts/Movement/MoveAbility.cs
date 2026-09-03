using System;
using UnityEngine;

public class MoveAbility : MonoBehaviour
{
    private TagSpeed tagSpeed;

    public float Direction { get; set; } = 0.0f;

    public float Speed
    {
        get
        {
            return tagSpeed.Speed;
        }
    }
    public float MoveVelocity
    {
        get
        {
            return Direction * Speed;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init(TagSpeed tagSpeed)
    {
        this.tagSpeed = tagSpeed;

        enabled = true;
    }
}
