using System;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    private Weapon weapon;

    public Weapon Weapon 
    {
        get
        {
            return weapon;
        }
    }

    private async void Awake()
    {
        GameObject weaponObject = await AssetLoader.ClonePrefabAsync(PrefabPaths.WeaponPrefab, transform);
        weaponObject.name = "Weapon";
        weapon = weaponObject.GetComponent<Weapon>();
    }

    public void SetWeapon(WeaponData weaponData)
    {
        weapon.WeaponData = weaponData;
    }
}
