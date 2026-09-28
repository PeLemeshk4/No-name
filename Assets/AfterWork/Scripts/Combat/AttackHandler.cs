using System;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    private HealthController healthController;
    private ParryAbility parryAbility;

    //private bool attacked = false;
    private Vector2 viewDirection = Vector2.zero;

    public event EventHandler<ValueChangedEventArgs<bool>> hasBeenAttacked;

    private void Awake()
    {
        enabled = false;
    }

    public void Init(LookAbility lookAbility, HealthController healthController, ParryAbility parryAbility)
    {
        this.healthController = healthController;
        this.parryAbility = parryAbility;

        viewDirection = lookAbility.Direction;
        lookAbility.directionChanged += OnChangeDirectionLook;
        enabled = true;
    }

    public void TryProcessAttack(float damage, Transform attackTransform, IParryable parryable)
    {
        if (parryAbility != null && parryAbility.TryParry(attackTransform.gameObject, parryable))
        {
            Debug.Log("Успешное парирование!");
            return;
        }

        float scal = Vector2.Dot(viewDirection, (attackTransform.position - transform.position).normalized);
        float angleThreshold = Mathf.Cos(22.5f * Mathf.Deg2Rad);
        if (scal >= angleThreshold)
        {
            Debug.Log("Parry");
        }

        healthController?.TryConsume(damage);
        hasBeenAttacked?.Invoke(this, new ValueChangedEventArgs<bool>(false, true));
        //attacked = true;

        hasBeenAttacked?.Invoke(this, new ValueChangedEventArgs<bool>(false, true));
    }

    private void OnChangeDirectionLook(object s, ValueChangedEventArgs<Vector2> e)
    {
        viewDirection = e.Value;
    }
}
