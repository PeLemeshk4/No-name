using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponData weaponData;

    protected async Task LoadData(string path)
    {
        var operation = Addressables.LoadAssetAsync<WeaponData>(path);

        try
        {
            await operation.Task;
            weaponData = operation.Result;
        }
        catch
        {
            Debug.LogError($"Failed to load weapon data by link: {path}");
            weaponData = null;
        }       
    }

    public abstract void Attack();
}
