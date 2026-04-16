using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackHitEventsArgs : EventArgs
{
    public Weapon NewWeapon { get; }

    public AttackHitEventsArgs(Weapon newWeapon)
    {
        NewWeapon = newWeapon;
    }
}

public class Attack : MonoBehaviour
{
    private List<AttackHandler> collided = new List<AttackHandler>();

    public event EventHandler<AttackHitEventsArgs> AttackHit;

    private float damage = 0;

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    private void OnEnable()
    {
        collided.Clear();
    }

    private void LateUpdate()
    {
        if (collided.Count == 0) return;

        Transform self = transform;
        AttackHandler closest = null;
        float best = float.MaxValue;

        foreach (AttackHandler c in collided)
        {
            if (!c) continue;

            float d = Vector2.Distance(self.position, c.transform.position);
            if (d < best)
            {
                best = d;
                closest = c;
            }
        }

        if (closest)
        {
            closest.TryProcessAttack(damage);
            
        }          
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attackHandler)) return;

        collided.Add(attackHandler);
    }
}
