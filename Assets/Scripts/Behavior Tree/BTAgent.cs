using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    [SerializeField] GameObject treasure;
    [SerializeField] GameObject otherTreasure;
    NavMeshAgent agent;
    BehaviorTree tree;
    [SerializeField] List<Transform> waypoints = new();
    // Start is called before the first frame update
    void Start()
    {
        
        tree = new BehaviorTree("Enemy");

        BTLeaf isTreasurePresent = new BTLeaf("IsTreasurePresent", new Condition(() => treasure.activeSelf));
        BTLeaf moveToTreasure = new BTLeaf("MoveToTreasure", new ActionStrategy(() => agent.SetDestination(treasure.transform.position)));

        BTLeaf isOtherTreasurePresent = new BTLeaf("IsOtherTreasurePresent", new Condition(() => otherTreasure.activeSelf));
        BTLeaf moveToOtherTreasure = new BTLeaf("MoveToOtherTreasure", new ActionStrategy(() => agent.SetDestination(otherTreasure.transform.position)));

        BTSequence goToTreasure = new BTSequence("GoToTreasure");

        goToTreasure.AddChild(isTreasurePresent);
        goToTreasure.AddChild(moveToTreasure);

        BTSequence goToOtherTreasure = new BTSequence("GoToOtherTreasure");

        goToOtherTreasure.AddChild(isOtherTreasurePresent);
        goToOtherTreasure.AddChild(moveToOtherTreasure);

        BTSelector goToEitherTreasure = new BTSelector("GoToEitherTreasure");
        goToEitherTreasure.AddChild(goToTreasure);
        goToEitherTreasure.AddChild(goToOtherTreasure);

        tree.AddChild(new BTLeaf("Patrol", new PatrolStrategy(transform, agent, waypoints)));
        tree.AddChild(goToEitherTreasure);
        

    }

    // Update is called once per frame
    void Update()
    {
        tree.Process();
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

}
