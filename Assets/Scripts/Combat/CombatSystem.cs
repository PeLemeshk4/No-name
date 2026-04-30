using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;

    private void Awake()
    {
        TryGetComponent<MovementSystem>(out movementSystem);
    }

    public bool TryAttack(Weapon weapon)
    {
        if (movementSystem != null && movementSystem.IsDash) return false;
        if (weapon == null) return false;

        weapon.Attack();
        return true;
    }
}
