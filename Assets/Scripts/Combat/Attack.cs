using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private List<AttackHandler> attacked = new List<AttackHandler>();

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
        attacked.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackHandler attackHandler;
        if (collision.gameObject.TryGetComponent<AttackHandler>(out attackHandler))
        {
            if (attacked.Contains(attackHandler)) return;

            attackHandler.TryProcessAttack(damage);
            attacked.Add(attackHandler);
        }
    }
}
