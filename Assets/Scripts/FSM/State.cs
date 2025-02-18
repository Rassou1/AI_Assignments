using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class State 
{
    protected FSMAgent agent;
    protected StateMachine stateMachine;
    
    protected Transform player;
    protected Transform station;
    
    protected Transform transform;

    public State(FSMAgent agent, StateMachine stateMachine)
    {
        this.agent = agent;
        this.stateMachine = stateMachine;
        transform = agent.transform;

        player = agent.player;
        station = agent.station;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }

}

public class IdleState : State
{
    public IdleState(FSMAgent agent, StateMachine stateMachine) : base(agent, stateMachine)
    {
        
    }

    public override void EnterState()
    {
        if (!transform.gameObject.activeSelf)
            transform.gameObject.SetActive(true);
        Debug.Log("Idling, but in FSM");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("No longer idling.");
        base.ExitState();
    }

    public override void Update()
    {
        if (agent.Track)
        {
            stateMachine.ChangeState(agent.trackState);
        }
        else if (agent.Dock)
        {
            stateMachine.ChangeState(agent.dockState);
        }
        base.Update();
    }
}


public class TrackState : State
{
    public TrackState(FSMAgent agent, StateMachine stateMachine) : base(agent, stateMachine)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Tracking the player.");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("No longer tracking.");
        base.ExitState();
    }

    public override void Update()
    {
        agent.position = Vector3.MoveTowards(agent.position, player.position, Time.deltaTime * 2f);
        if (!agent.Track)
        {
            stateMachine.ChangeState(agent.dockState);
        }
        base.Update();
    }
}

public class DockState : State
{
    public DockState(FSMAgent agent, StateMachine stateMachine) : base(agent, stateMachine)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Moving to dock.");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("No longer moving to dock.");
        base.ExitState();
    }

    public override void Update()
    {
        agent.position = Vector3.MoveTowards(agent.position, station.position, Time.deltaTime * 2f);
        if (agent.Track)
        {
            stateMachine.ChangeState(agent.trackState);
        }
        else if (!agent.Dock)
        {
            stateMachine.ChangeState(agent.idleState);
        }
        base.Update();
    }
}