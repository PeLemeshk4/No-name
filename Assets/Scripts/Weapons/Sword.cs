using UnityEngine;

public class Sword : Weapon
{

    private async void Awake()
    {
        await LoadData(DataPaths.SwordData);
    }

    public override void Attack()
    {
        Debug.Log(weaponData.Damage);
    }
}
