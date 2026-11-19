using System;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private HealthController healthController;

    public event EventHandler<ValueChangedEventArgs<bool>> hasBeenAttacked;

    private void Awake()
    {
        enabled = false;
    }
    public void Init(HealthController healthController)
    {
        this.healthController = healthController;

        enabled = true;
    }

    public void TryProcessAttack(float damage, Transform attackTransform)
    {
        healthController?.TryConsume(damage);

        hasBeenAttacked?.Invoke(this, new ValueChangedEventArgs<bool>(false, true));
    }
}
