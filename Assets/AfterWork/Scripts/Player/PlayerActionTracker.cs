using System;
using UnityEngine;

public class PlayerActionTracker
{
    private bool isTracking = false;

    public float TotalTime { get; private set; } = 0;
    public int DashCount { get; private set; } = 0;
    public int AttackCount { get; private set; } = 0;

    public event EventHandler<EventArgs> Changed;

    public PlayerActionTracker(DashAbility dashAbility, AttackAbility attackAbility)
    {
        dashAbility.IsDashed += Dash;
        attackAbility.IsAttacked += Attack;
    }

    public void StartTracking()
    {
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    public void ResetData()
    {
        DashCount = 0;
        AttackCount = 0;
    }

    private void Dash(object o, EventArgs e)
    {
        DashCount++;
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void Attack(object o, EventArgs e)
    {
        AttackCount++;
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
