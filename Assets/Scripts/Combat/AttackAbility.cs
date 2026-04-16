using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;

    [SerializeField] private ActiveWeapon activeWeapon;

    private void Start()
    {
        combatSystem = GetComponent<CombatSystem>();

        activeWeapon = GetComponent<ActiveWeapon>();
    }

    public void Attack(Vector2 direction)
    {
        combatSystem.TryAttack(activeWeapon.Weapon, direction);
    }
}
