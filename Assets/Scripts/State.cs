using System;
using UnityEngine;

[Serializable]
public abstract class State
{
    public bool isActive { get; set; } = true;

    public static implicit operator bool(State state)
    {
        return state is { isActive: true };
    }


    public virtual void EnterState()
    {

    }
    public virtual void UpdateState()
    {

    }
    public virtual void ExitState()
    {

    }
}
