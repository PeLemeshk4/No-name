using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackAbility : MonoBehaviour
{
    public event EventHandler<EventArgs> IsAttacked;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        enabled = true;
    }

    public bool Attack(Weapon weapon, Vector2 direction)
    {
        if (weapon  == null) return false;
        if (direction == Vector2.zero) return false;

        if (weapon.Attack(direction))
        {
            IsAttacked?.Invoke(this, new EventArgs());
            return true;
        }
        else return false;
    }
}
