using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected Transform transform;
    protected StateMachine stateMachine;

    public State(Transform transform, StateMachine stateMachine)
    {
        this.transform = transform;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }

}

public class IdleState : State
{
    public IdleState(Transform transform, StateMachine stateMachine) : base(transform, stateMachine)
    {

    }
}


public class TrackState : State
{
    public TrackState(Transform transform, StateMachine stateMachine) : base(transform, stateMachine)
    {

    }
}

public class AttackState : State
{
    public AttackState(Transform transform, StateMachine stateMachine) : base(transform, stateMachine)
    {

    }
}