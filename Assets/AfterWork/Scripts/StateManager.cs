using System;
using System.Collections.Generic;

public enum States
{
    Idle,
    Run,
    Jump,
    Fall,
    Dash,
    Attack,
    Dead
}

public class StateManager
{
    private Dictionary<States, Func<bool>> blockedStates = new Dictionary<States, Func<bool>>();

    public States CurrentState { private set; get; }
    public event EventHandler<ValueChangedEventArgs<States>> StateChanged;

    public bool SetState(States state)
    {
        StateChanged?.Invoke(this, new ValueChangedEventArgs<States>(CurrentState, state));
        CurrentState = state;

        return true;
    }

    public bool StateBlocked()
    {
        return blockedStates.TryGetValue(CurrentState, out Func<bool> isBlocked) && isBlocked();
    }

    public bool AddBlockedState(States state, Func<bool> isState)
    {
        if (isState == null) return false;
        if (blockedStates.ContainsKey(state)) return false;

        blockedStates.Add(state, isState);
        return true;
    }

    public StateManager(States state)
    {
        CurrentState = state;
    }
}
