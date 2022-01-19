using System;
using System.Collections;
using System.Collections.Generic;

public abstract class StateMachine
{
    protected Dictionary<Type, HashSet<Type>> transitions = new Dictionary<Type, HashSet<Type>>();
    public Dictionary<Type, HashSet<Type>> Transitions { get => transitions; }

    public State CurrState { get; private set; }

    private bool CanTransitionTo(State state)
    {
        return CurrState == null || transitions[CurrState.GetType()].Contains(state.GetType());
    }

    public void ChangeState(State newState)
    {
        if (CanTransitionTo(newState))
        {
            CurrState?.Exit();
            CurrState = newState;
            CurrState.Enter();
        }
    }

    public virtual void Update()
    {
        CurrState?.Execute();
    }

    public void Exit()
    {
        CurrState?.Exit();
        CurrState = null;
    }
}
