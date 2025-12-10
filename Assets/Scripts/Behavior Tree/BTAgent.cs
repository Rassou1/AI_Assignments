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
    [SerializeField] Transform player;
    public float detectionDistance = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
        tree = new BehaviorTree("Enemy");

        BTSelector root = new BTSelector("Root");

        BTLeaf isTreasurePresent = new BTLeaf("IsTreasurePresent", new Condition(() => treasure.activeSelf));
        BTLeaf moveToTreasure = new BTLeaf("MoveToTreasure", new ActionStrategy(() => agent.SetDestination(treasure.transform.position)));

        BTLeaf isOtherTreasurePresent = new BTLeaf("IsOtherTreasurePresent", new Condition(() => otherTreasure.activeSelf));
        BTLeaf moveToOtherTreasure = new BTLeaf("MoveToOtherTreasure", new ActionStrategy(() => agent.SetDestination(otherTreasure.transform.position)));

        BTLeaf patrol = new BTLeaf("Patrol", new PatrolStrategy(transform, agent, waypoints));

        BTLeaf isPlayerInRange = new BTLeaf("IsPlayerInRange", new Condition(() => Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z)) < detectionDistance));

        BTInverter inv_PlayerInRange = new BTInverter("InvertGoToTreasure");

        inv_PlayerInRange.AddChild(isPlayerInRange);
        
        BTSequence goToTreasure = new BTSequence("GoToTreasure");
        goToTreasure.AddChild(inv_PlayerInRange); //activate to show inverter working
        goToTreasure.AddChild(isTreasurePresent);
        goToTreasure.AddChild(moveToTreasure);

        BTSequence goToOtherTreasure = new BTSequence("GoToOtherTreasure");

        goToOtherTreasure.AddChild(isOtherTreasurePresent);
        goToOtherTreasure.AddChild(moveToOtherTreasure);

        BTSelector goToEitherTreasure = new BTSelector("GoToEitherTreasure");
        goToEitherTreasure.AddChild(goToTreasure);
        goToEitherTreasure.AddChild(goToOtherTreasure);

        #region normal working bt
        root.AddChild(goToEitherTreasure);

        root.AddChild(patrol);

        tree.AddChild(root);
        #endregion

        #region bt with inverter demo

        //BTSelector selectBasedOnPlayer = new BTSelector("SelectBasedOnPlayer");

        //BTSequence playerInRange = new BTSequence("PlayerInRange");
        //playerInRange.AddChild(isPlayerInRange);
        //playerInRange.AddChild(isOtherTreasurePresent);
        //playerInRange.AddChild(moveToOtherTreasure);

        //BTSequence playerNotInRange = new BTSequence("PlayerNotInRange");

        //BTInverter invertPlayerInRange = new BTInverter("InvertPlayerInRange");
        //invertPlayerInRange.AddChild(isPlayerInRange);

        //BTSelector treasureWhenNoPlayer = new BTSelector("TreasureWhenNoPlayer");

        //BTSequence tryTreasure = new BTSequence("TryTreasure");
        //tryTreasure.AddChild(isTreasurePresent);
        //tryTreasure.AddChild(moveToTreasure);

        //BTSequence tryOtherTreasure = new BTSequence("TryOtherTreasure");
        //tryOtherTreasure.AddChild(isOtherTreasurePresent);
        //tryOtherTreasure.AddChild(moveToOtherTreasure);

        //treasureWhenNoPlayer.AddChild(tryTreasure);
        //treasureWhenNoPlayer.AddChild(tryOtherTreasure);

        //playerNotInRange.AddChild(invertPlayerInRange);
        //playerNotInRange.AddChild(treasureWhenNoPlayer);

        //selectBasedOnPlayer.AddChild(playerInRange);
        //selectBasedOnPlayer.AddChild(playerNotInRange);

        //root.AddChild(selectBasedOnPlayer);
        //root.AddChild(patrol);

        //tree.AddChild(root);

        #endregion

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
