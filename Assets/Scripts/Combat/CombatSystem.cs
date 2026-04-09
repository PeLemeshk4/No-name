using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private TimeSlowAbility timeSlowAbility;
    [SerializeField] private MovementSystem movementSystem;

    private void Start()
    {
        TryGetComponent<TimeSlowAbility>(out timeSlowAbility);
        TryGetComponent<MovementSystem>(out movementSystem);
    }

    public bool TryAttack(Weapon weapon, Vector2 direction)
    {
        if (timeSlowAbility != null && timeSlowAbility.IsActive) return false;
        if (movementSystem != null && movementSystem.IsDash) return false;
        if (weapon == null) return false;
        weapon.Attack(direction);
        return true;
    }
}
