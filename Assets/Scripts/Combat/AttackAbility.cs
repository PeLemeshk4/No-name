using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;

    [SerializeField] private ActiveWeapon activeWeapon;

    private Weapon weapon;

    private void Start()
    {
        combatSystem = GetComponent<CombatSystem>();

        activeWeapon = GetComponent<ActiveWeapon>();
        activeWeapon.ChangeWeapon += OnWeaponChanged;

        weapon = activeWeapon.Weapon;
    }

    public void Attack(Vector2 direction)
    {
        combatSystem.TryAttack(weapon, direction);
    }

    private void OnWeaponChanged(object sender, WeaponChangedEventArgs e)
    {
        weapon = e.NewWeapon;
    }
}
