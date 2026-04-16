using System.Threading.Tasks;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Weapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected Attack attack;
    protected bool isAttacking = false;

    public WeaponData WeaponData
    {
        get
        {
            return weaponData;
        }
        set
        {
            weaponData = value;
            UpdateWeapon();
        }
    }

    private void Awake()
    {
        attack = gameObject.AddComponent<Attack>();
        attack.enabled = false;
    }

    private void UpdateWeapon()
    {
        attack.Damage = weaponData.Damage;
    }

    public void Attack(Vector2 attackDirection)
    {
        if (weaponData == null) return;
        if (isAttacking) return;

        float weaponRotate;

        if (Mathf.Abs(attackDirection.x) >= Mathf.Abs(attackDirection.y))
            weaponRotate = attackDirection.x >= 0 ? -90.0f : 90.0f;
        else
            weaponRotate = attackDirection.y >= 0 ? 0.0f : 180.0f;

        transform.eulerAngles = new Vector3(0, 0, weaponRotate);

        isAttacking = true;
        attack.enabled = true;

        Debug.Log(weaponData.Damage);
    }
}
