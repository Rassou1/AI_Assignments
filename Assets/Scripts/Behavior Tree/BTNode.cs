using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BehaviorTree : BTNode
{
    public BehaviorTree(string name) : base(name) {}

    public override Status Process()
    {
        while (currentChild < children.Count)
        {
            var status = children[currentChild].Process();
            if(status != Status.SUCCESS)
            {
                return status;
            }
            currentChild++;
        }
        return Status.SUCCESS;
    }
}


public class BTLeaf : BTNode
{
    readonly IStrategy strategy;

    public BTLeaf(string name, IStrategy strategy) : base(name)
    {
        this.strategy = strategy;
    }

    public override Status Process() => strategy.Process();

    public override void Reset() => strategy.Reset();
}

public class BTNode
{
    public enum Status { SUCCESS, FAILURE, RUNNING }

    public readonly string name;

    public readonly List<BTNode> children = new List<BTNode>();
    protected int currentChild;

    public BTNode(string name)
    {
        this.name = name;
    }

    public void AddChild(BTNode child)
    {
        children.Add(child);
    }

    public virtual Status Process() => children[currentChild].Process();

    public virtual void Reset() 
    {
        currentChild = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }
    
}
