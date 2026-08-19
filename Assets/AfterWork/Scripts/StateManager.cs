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
    private Dictionary<States, Func<bool>> lockedStates = new Dictionary<States, Func<bool>>();

    public States CurrentState { private set; get; }
    public event EventHandler<ValueChangedEventArgs<States>> StateChanged;

    public bool SetState(States state)
    {
        StateChanged?.Invoke(this, new ValueChangedEventArgs<States>(CurrentState, state));
        CurrentState = state;

        return true;
    }

    public bool IsStateLocked()
    {
        return lockedStates.TryGetValue(CurrentState, out Func<bool> isLocked) && isLocked();
    }

    public bool AddBlockedState(States state, Func<bool> isState)
    {
        if (isState == null) return false;
        if (lockedStates.ContainsKey(state)) return false;

        lockedStates.Add(state, isState);
        return true;
    }

    public StateManager(States state)
    {
        CurrentState = state;
    }
}
