using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAgent : MonoBehaviour
{
    public Transform player;
    public Transform station;
    public Vector3 position;

    public StateMachine stateMachine = new StateMachine();
    public State state;

    public IdleState idleState;
    public DockState dockState;
    public TrackState trackState;

    public float detectionDistance = 5f;

    public bool Track;
    public bool Dock;

    // Start is called before the first frame update
    void Start()
    {
        idleState = new IdleState(this, stateMachine);
        dockState = new DockState(this, stateMachine);
        trackState = new TrackState(this, stateMachine);

        position = transform.position;

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z)) < detectionDistance))
            Track = false;
        else Track = true;
        
        if (!(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(station.position.x, station.position.z)) > 1f))
            Dock = false;
        else Dock = true;
       

        transform.position = position;

        stateMachine.currentState.Update();
    }
}
