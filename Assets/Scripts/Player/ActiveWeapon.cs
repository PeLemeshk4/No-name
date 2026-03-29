using System;
using UnityEngine;

public class WeaponChangedEventArgs : EventArgs
{
    public Weapon NewWeapon { get; }

    public WeaponChangedEventArgs(Weapon newWeapon)
    {
        NewWeapon = newWeapon;
    }
}

public class ActiveWeapon : MonoBehaviour
{
    private Weapon weapon;

    public event EventHandler<WeaponChangedEventArgs> ChangeWeapon;

    public Weapon Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
            ChangeWeapon?.Invoke(this, new WeaponChangedEventArgs(value));
        }
    }
}
