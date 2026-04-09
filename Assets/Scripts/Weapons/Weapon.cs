using System.Threading.Tasks;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Weapon : MonoBehaviour
{
    protected abstract string Path { get; }

    protected WeaponData weaponData;
    protected Attack attack;

    protected bool isAttacking = false;

    private async void Awake()
    {
        await LoadData();

        attack = gameObject.AddComponent<Attack>();
        attack.Damage = weaponData.Damage;
        attack.enabled = false;
    }

    protected async Task LoadData()
    {
        var operation = Addressables.LoadAssetAsync<WeaponData>(Path);

        try
        {
            await operation.Task;
            weaponData = operation.Result;
        }
        catch
        {
            Debug.LogError($"Failed to load weapon data by link: {Path}");
            weaponData = null;
        }
    }

    public abstract void Attack(Vector2 attackDirection);
}
