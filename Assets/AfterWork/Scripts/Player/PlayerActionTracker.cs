using System;
using UnityEngine;

public class PlayerActionTracker : MonoBehaviour
{
    private bool isTracking = false;

    private bool isIdle = false;

    public float TotalTime { get; private set; } = 0;
    public int DashCount { get; private set; } = 0;
    public int AttackCount { get; private set; } = 0;
    public float IdleTime { get; private set; } = 0;

    public event EventHandler<EventArgs> Changed;

    public void Awake()
    {
        enabled = false;
    }
    public void Init(DashAbility dashAbility, AttackAbility attackAbility, StateManager sM)
    {
        dashAbility.IsDashed += Dash;
        attackAbility.IsAttacked += Attack;
        sM.StateChanged += StateChanged;
        if (sM.CurrentState == States.Idle) isIdle = true;

        enabled = true;
    }

    private void Update()
    {
        if (!isTracking) return;

        if (isIdle) IdleTime += Time.deltaTime;
        TotalTime += Time.deltaTime;
        Changed?.Invoke(this, EventArgs.Empty);
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
        TotalTime = 0.0f;
        DashCount = 0;
        AttackCount = 0;
        IdleTime = 0.0f;

        Changed?.Invoke(this, EventArgs.Empty);
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

    private void StateChanged(object o, ValueChangedEventArgs<States> e)
    {
        if (e.Value == States.Idle) isIdle = true;
        else isIdle = false;
    }

}
