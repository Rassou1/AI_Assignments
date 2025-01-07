using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionDistance = 5f;
    private DecisionNode rootnode;

    // Start is called before the first frame update
    void Start()
    {
        var chasePlayer = new Action(() => Chase());
        var idle = new Action(() => Idle());

        var chaseNode = new LeafNode(chasePlayer);
        var idleNode = new LeafNode(idle);

        var playerWithinReach = new DecisionNode(
            () => Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z)) < detectionDistance,
            chaseNode,
            idleNode);

        rootnode = playerWithinReach;
    }

    // Update is called once per frame
    void Update()
    {
        rootnode.Evaluate();
    }

    private void Chase()
    {
        Debug.Log("Player spotted, now chasing");
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 2f);
    }

    private void Idle()
    {
        Debug.Log("Nothing to see here, move along");
    }
}


public abstract class DecisionTreeNode
{
    public abstract void Evaluate();
}

public class DecisionNode : DecisionTreeNode
{
    public System.Func<bool> Condition;
    public DecisionTreeNode ifTrue;
    public DecisionTreeNode ifFalse;

    public DecisionNode(System.Func<bool> condition, DecisionTreeNode ifTrue, DecisionTreeNode ifFalse)
    {
        Condition = condition;
        this.ifTrue = ifTrue;
        this.ifFalse = ifFalse;
    }

    public override void Evaluate()
    {
        if (Condition())
        {
            ifTrue.Evaluate();
        }
        else
        {
            ifFalse.Evaluate();
        }
    }

}
public class LeafNode : DecisionTreeNode
{
    private Action action;
    public LeafNode(Action action)
    {
        this.action = action;
    }

    public override void Evaluate()
    {
        action.Invoke();
    }
}