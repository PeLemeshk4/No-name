using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TryGetComponent<AttackHandler>(out AttackHandler attackHandler))
        {
            attackHandler.TryProcessAttack(damage, transform);
        }
    }
}
