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
            switch(children[currentChild].Process())
            {
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.SUCCESS:
                    currentChild++;
                    return currentChild < children.Count ? Status.RUNNING : Status.SUCCESS;
                case Status.FAILURE:
                    Reset();
                    return Status.FAILURE;
            }
        }
        Reset();
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

public class BTSequence : BTNode
{
    public BTSequence(string name) : base(name) { }

    public override Status Process()
    {
        while (currentChild < children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.FAILURE:
                    currentChild = 0;
                    return Status.FAILURE;
                case Status.SUCCESS:
                    currentChild++;
                    return currentChild == children.Count ? Status.SUCCESS : Status.RUNNING;
            }
        }

        Reset();
        return Status.SUCCESS;
    }
}

public class BTSelector : BTNode
{
    public BTSelector(string name) : base(name) { }

    public override Status Process()
    {
        while (currentChild < children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.RUNNING:
                    return Status.RUNNING;
                case Status.SUCCESS:
                    Reset();
                    return Status.SUCCESS;
                case Status.FAILURE:
                    currentChild++;
                    return Status.RUNNING;
            }
        }

        Reset();
        return Status.FAILURE;
    }
}

public class BTInverter : BTNode
{
    public BTInverter(string name) : base(name) { }

    public override Status Process()
    {
         switch (children[0].Process())
         {
             case Status.RUNNING:
                 return Status.RUNNING;
             case Status.SUCCESS:
                 Reset();
                 Debug.Log("Child returned success, we returned failure");
                 return Status.FAILURE;
             case Status.FAILURE:
                 Reset();
                 Debug.Log("Child returned failure, we returned success");
                 return Status.SUCCESS;
         }

        Reset();
        return Status.SUCCESS;

    }
}
