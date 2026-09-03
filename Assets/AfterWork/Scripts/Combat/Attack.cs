using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackHitEventsArgs : EventArgs
{
    public GameObject Target { get; }

    public AttackHitEventsArgs(GameObject target)
    {
        Target = target;
    }
}

public class Attack : MonoBehaviour
{
    public event EventHandler<AttackHitEventsArgs> AttackHit;

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
        enabled = false;
    }
    public void Init(Collider2D c)
    {
        attackCollider = c;
        attackCollider.enabled = false;

        enabled = true;
    }

    private void OnEnable()
    {
        attackCollider.enabled = true;
    }

    private void OnDisable()
    {
        if (attackCollider == null) return;

        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == gameObject) return;
        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attackHandler))
        {
            attackHandler.TryProcessAttack(damage, transform);
            AttackHit?.Invoke(this, new AttackHitEventsArgs(collision.gameObject));
        }
    }
}
