using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponData weaponData;

    protected void LoadData(string name)
    {
        string path = "";
        //weaponData = Addressables.LoadAssetAsync<WeaponData>(path);
    }

    public abstract void Attack();
}
