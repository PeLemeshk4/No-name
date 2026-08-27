using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        enabled = true;
    }

    public bool Attack(Weapon weapon, Vector2 direction)
    {
        if (weapon  == null) return false;
        if (direction == Vector2.zero) return false;

        return weapon.Attack(direction);
    }
}
