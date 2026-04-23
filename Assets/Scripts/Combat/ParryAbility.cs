using UnityEngine;

public class ParryAbility : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;

    private void Awake()
    {
        combatSystem = GetComponent<CombatSystem>();
    }

    public void Parry(Weapon weapon)
    {
        combatSystem.TryParry(weapon);
    }
}
