using UnityEngine;

public class Sword : Weapon
{
    private Attack attack;
    private bool isAttacking = false;

    private async void Awake()
    {
        await LoadData(DataPaths.SwordData);

        attack = gameObject.AddComponent<Attack>();
        attack.Damage = weaponData.Damage;
        attack.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            transform.position += new Vector3(1, 0, 0);
            isAttacking = false;
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        attack.enabled = true;

        Debug.Log(weaponData.Damage);
    }
}
