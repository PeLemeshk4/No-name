using System;
using UnityEngine;

public class AttackAbility
{
    public event EventHandler<EventArgs> IsAttacked;

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
