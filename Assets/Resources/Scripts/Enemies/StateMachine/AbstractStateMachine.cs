using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


//T - enum of states names
public abstract class AbstractStateMachine<T> : MonoBehaviour, IStateMachineToDeath
{


    protected Dictionary<T, AbstractState<T>> _states = new Dictionary<T, AbstractState<T>>();

    protected EnemyInterface thisEnemy;
    public EnemyInterface GetThisEnemy => thisEnemy;
    
    protected AbstractState<T> _currentState;


    protected abstract void CreateStates();


    protected virtual void AddToDictionary(AbstractState<T> state)
    {
        _states.Add(state._thisState, state);
    }
    

    protected virtual void Start()
    {
        thisEnemy = GetComponent<EnemyInterface>();
        CreateStates();
        _currentState.OnEnter();
    }


    protected virtual void Update()
    {
        _currentState.Updater();
    }

    public virtual void ChangeState(T newState)
    {
        if(_currentState != null)
            _currentState.OnExit();
        _currentState = _states[newState];
        _currentState.OnEnter();
    }


    public abstract void ChangeToDeathState();
}
