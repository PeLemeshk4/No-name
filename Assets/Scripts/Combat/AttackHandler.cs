using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private HealthController healthController;

    private void Start()
    {
        healthController = GetComponent<HealthController>();
    }

    public void TryProcessAttack(float damage)
    {
        healthController?.TryConsume(damage);
    }
}
