using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IParryable
{
    public event EventHandler<AttackHitEventsArgs> AttackHit;

    private Collider2D attackCollider;
    protected float damage = 0;

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

    public bool CanBeParried => true;

    public void OnParried(ParryContext context)
    {
        enabled = false;
        if (attackCollider != null) attackCollider.enabled = false;
    }

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        attackCollider.enabled = true;
    }

    private void OnDisable()
    {
        attackCollider.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == gameObject) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attackHandler))
        {
            attackHandler.TryProcessAttack(damage, transform, this);
            AttackHit?.Invoke(this, new AttackHitEventsArgs(collision.gameObject));
        }
    }
}
