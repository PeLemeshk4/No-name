using System;
using UnityEngine;

public class ParryAbility : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float parryDuration = 0.25f;
    [SerializeField] private float windowStart = 0.05f;
    [SerializeField] private float windowEnd = 0.18f;
    [SerializeField] private float cooldown = 0.3f;

    public bool IsParrying { get; private set; }
    public bool IsInWindow {  get; private set; }
    public bool CanStart => !IsParrying && cooldownTimer <= 0f;

    public event Action<ParryContext> Parried;
    public event Action ParryEnded;

    private float timer;
    private float cooldownTimer;
    private bool successThisParry;

    private void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
        if (!IsParrying) return;
        timer += Time.deltaTime;

        if (!IsInWindow && timer >= windowStart)
        {
            IsInWindow = true;
        }

        if (IsInWindow && timer >= windowEnd)
        {
            IsInWindow = false;
        }
        if (timer >= parryDuration)
        {
            EndParry();
        }
    }

    public bool TryStartParry()
    {
        if (!CanStart) return false;

        IsParrying = true;
        IsInWindow = false;
        successThisParry = false;
        timer = 0f;
        return true;
    }
    public bool TryParry(GameObject attacker, IParryable target)
    {
        if (!IsParrying || !IsInWindow || successThisParry) return false;
        if (target == null || !target.CanBeParried) return false;

        successThisParry = true;
        cooldownTimer = cooldown;
        IsInWindow = false;

        var ctx = new ParryContext(gameObject, Vector2.zero, transform.position, target);
        target.OnParried(ctx);
        Parried?.Invoke(ctx);

        return true;

    }
    private void EndParry()
    {
        IsParrying = false;
        IsInWindow = false;
        ParryEnded?.Invoke();
    }
}
