using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private HealthController healthController;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    public void TryProcessAttack(float damage)
    {
        healthController?.TryConsume(damage);
        Debug.Log(gameObject.name + ": " + damage);
    }
}
