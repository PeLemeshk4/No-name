using UnityEngine;

public class Spear : Weapon
{
    protected override string Path => DataPaths.SpearData;

    public override void Attack(Vector2 attackDirection)
    {
        Debug.Log(weaponData.Damage);
    }
}
