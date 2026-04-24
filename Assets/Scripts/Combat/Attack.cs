using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackHitEventsArgs : EventArgs
{
    public GameObject Target { get; }

    public List<Attack> Attacks {  get; }

    public AttackHitEventsArgs(GameObject target, List<Attack> attacks)
    {
        Target = target;
        Attacks = attacks;
    }
}

public class Attack : MonoBehaviour
{
    public event EventHandler<AttackHitEventsArgs> AttackHit;

    private List<AttackHandler> collided = new List<AttackHandler>();
    private List<Attack> attacks = new List<Attack>();
    private Collider2D attackCollider;
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

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        collided.Clear();
        attacks.Clear();
    }

    private void Update()
    {
        Debug.Log(enabled);
        Debug.Log(2);
        CheckForActiveContacts();
        Debug.Log(3);
        if (attacks.Count != 0)
        {
            AttackHit?.Invoke(this, new AttackHitEventsArgs(null, attacks));
        }

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
            AttackHit?.Invoke(this, new AttackHitEventsArgs(closest?.gameObject, null));
        }
    }

    private void CheckForActiveContacts()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCollider(attackCollider, colliders);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject == gameObject) return;
            if (c.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attackHandler))
            {
                collided.Add(attackHandler);
            }
            else if (c.gameObject.TryGetComponent<Attack>(out Attack attack))
            {
                attacks.Add(attack);
            }
        }
        Debug.Log(1);
    }
}
