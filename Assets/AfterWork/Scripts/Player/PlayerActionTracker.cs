using System;
using UnityEngine;

public class PlayerActionTracker : MonoBehaviour
{
    private bool isTracking = false;

    private States state;

    public float TotalTime { get; private set; } = 0.0f;
    public int DashCount { get; private set; } = 0;
    public int AttackCount { get; private set; } = 0;
    public float IdleTime { get; private set; } = 0.0f;
    public float StealthTime { get; private set; } = 0.0f;
    public int KilledEnemy { get; private set; } = 0;

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
        state = sM.CurrentState;

        enabled = true;
    }

    private void Update()
    {
        if (!isTracking) return;

        switch (state)
        {
            case States.Idle:
                IdleTime += Time.deltaTime;
                break;
            case States.StealthRun:
                StealthTime += Time.deltaTime;
                break;
            case States.Stealth:
                StealthTime += Time.deltaTime;
                break;
        }
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
        StealthTime = 0.0f;
        KilledEnemy = 0;

        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void Dash(object o, EventArgs e)
    {
        if (!isTracking) return;

        DashCount++;
    }

    private void Attack(object o, EventArgs e)
    {
        if (!isTracking) return;

        AttackCount++;
    }

    public void EnemyDead(object o, EventArgs e)
    {
        if (!isTracking) return;

        KilledEnemy++;
    }

    private void StateChanged(object o, ValueChangedEventArgs<States> e)
    {
        state = e.Value;
    }

}
