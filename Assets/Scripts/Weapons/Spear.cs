using UnityEngine;

public class Spear : Weapon
{
    private async void Awake()
    {
        await LoadData(DataPaths.SpearData);
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
