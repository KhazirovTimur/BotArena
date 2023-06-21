using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractState<T>
{
    protected AbstractStateMachine<T> _stateMachine;
    public T _thisState;

    public AbstractState(AbstractStateMachine<T> stateMachine, T thisState)
    {
        _stateMachine = stateMachine;
        _thisState = thisState;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnExit()
    {
    }

}
