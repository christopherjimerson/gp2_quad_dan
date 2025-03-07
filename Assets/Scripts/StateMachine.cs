using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private List<State> _states;
    private State _currentState;

    private void OnEnable()
    {
        _states = new List<State>();

        // Find all types that inherit from State
        Type stateType = typeof(State);
        var stateSubclasses = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(stateType) && !type.IsAbstract);

        // Instantiate and add each subclass of State
        foreach (Type subclass in stateSubclasses)
        {
            if (Activator.CreateInstance(subclass) is State instance)
            {
                _states.Add(instance);
            }
        }

    }

    public void SwitchState<TAState>()
    {
        foreach (State s in _states)
        {
            if (s.GetType() != typeof(TAState)) continue;
            _currentState?.ExitState();
            _currentState = s;
            _currentState.EnterState();
            EventManager.Instance.StateSwapped(_currentState);
        }
    }

    protected virtual void UpdateStateMachine()
    {
        _currentState?.UpdateState();
    }

    public bool IsState<TAState>()
    {
        if (!_currentState) return false;
        return _currentState.GetType() == typeof(TAState);
    }
}
