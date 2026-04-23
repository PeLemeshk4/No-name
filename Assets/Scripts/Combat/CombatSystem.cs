using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private TimeSlowAbility timeSlowAbility;
    [SerializeField] private MovementSystem movementSystem;

    private void Awake()
    {
        TryGetComponent<TimeSlowAbility>(out timeSlowAbility);
        TryGetComponent<MovementSystem>(out movementSystem);
    }

    public bool TryAttack(Weapon weapon)
    {
        if (timeSlowAbility != null && timeSlowAbility.IsActive) return false;
        if (movementSystem != null && movementSystem.IsDash) return false;
        if (weapon == null) return false;
        weapon.Attack();
        return true;
    }

    public bool TryParry(Weapon weapon)
    {
        if (timeSlowAbility == null || !timeSlowAbility.IsActive) return false;
        if (movementSystem != null && movementSystem.IsDash) return false;
        if (weapon == null) return false;
        weapon.Parry();
        return true;
    }
}
