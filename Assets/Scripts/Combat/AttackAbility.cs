using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    private CombatSystem combatSystem;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        combatSystem = GetComponent<CombatSystem>();

        enabled = true;
    }

    public bool Attack(Weapon weapon, Vector2 direction)
    {
        return combatSystem.TryAttack(weapon, direction);
    }
}
