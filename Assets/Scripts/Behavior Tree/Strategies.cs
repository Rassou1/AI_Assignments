using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public interface IStrategy
{
    BTNode.Status Process();
    void Reset()
    {
        //Default implementation does nothing so we don't have one billion empty methods
    }
}

public class Condition : IStrategy
{
    readonly Func<bool> predicate;
    
    public Condition(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public BTNode.Status Process() => predicate() ? BTNode.Status.SUCCESS : BTNode.Status.FAILURE;
}

public class ActionStrategy : IStrategy
{
    readonly Action doSomething;

    public ActionStrategy(Action doSomething) 
    {
        this.doSomething = doSomething;
    }

    public BTNode.Status Process()
    {
        doSomething();
        return BTNode.Status.SUCCESS;
    }

}

public class PatrolStrategy : IStrategy
{
    readonly Transform entity;
    readonly NavMeshAgent agent;
    readonly List<Transform> patrolPoints;
    readonly float patrolSpeed;
    int currentIndex;
    bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
    {
        this.entity = entity;
        this.agent = agent;
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
    }

    public BTNode.Status Process()
    {
        if (currentIndex == patrolPoints.Count)
            return BTNode.Status.SUCCESS;

        var target = patrolPoints[currentIndex];
        agent.SetDestination(target.position);
        //entity.LookAt(target);

        if(isPathCalculated && agent.remainingDistance < 0.1f)
        {
            currentIndex++;
            isPathCalculated = false;
        }

        if (agent.pathPending)
        {
            isPathCalculated = true;
        }

        return BTNode.Status.RUNNING;
    }

    public void Reset() => currentIndex = 0;

}