using System;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    private Weapon weapon;
    private GameObject weaponObject;

    public Weapon Weapon 
    {
        get
        {
            return weapon;
        }
    }
    public GameObject WeaponObject
    {
        get
        {
            return weaponObject;
        }
    }

    private async void Awake()
    {
        weaponObject = await AssetLoader.ClonePrefabAsync(PrefabPaths.WeaponPrefab, transform);
        weaponObject.name = "Weapon";
        weapon = weaponObject.GetComponent<Weapon>();
    }

    public void SetWeapon(WeaponData weaponData)
    {
        weapon.WeaponData = weaponData;
    }

    public void SetWeaponDirection(Vector2 direction)
    {
        if (weapon.IsAttacking) return;

        float weaponAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        weaponObject.transform.eulerAngles = new Vector3(0, 0, weaponAngle);
    }
}
