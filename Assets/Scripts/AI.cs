using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour{

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<Intent> intentQueue = new List<Intent>();
    public Dictionary<GameObject, ArmyInfo> enemyArmyInfo = new Dictionary<GameObject, ArmyInfo>();
    GameObject unitShop;

    Faction faction;
    public TurnPhase turnPhase = TurnPhase.DefendAgainstThreats;
    public bool readyToExecute = true;
    bool moreToDoInPhase = false;


    // Start is called before the first frame update
    void Start(){
        CollectInformation();
    }

    void CollectInformation() {
        armies = GetComponent<Player>().armies;
        ownedNodes = GetComponent<Player>().ownedNodes;
        faction = GetComponent<Player>().faction;
    }

    // Update is called once per frame
    void Update(){
        print("Ready to Execute = :" + readyToExecute);
        print("Armies are ready: " + Army.readyToMove);
        if (TurnManager.currentPlayer == gameObject){
            if (readyToExecute){
                if (turnPhase == TurnPhase.DefendAgainstThreats) {
                    print("defend phase");
                    Defend();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Scouting;
                }
                else if (turnPhase == TurnPhase.Scouting) {
                    Scout();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Allocating;
                }
                else if (turnPhase == TurnPhase.Allocating) {
                    print("spend phase");
                    BuyUnits();
                    turnPhase = TurnPhase.Attacks;
                }
                else if (turnPhase == TurnPhase.Attacks) {
                    //print("attack phase");
                    //Attack();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Done;
                }
                else if (turnPhase == TurnPhase.Done) {
                    print("Turn Over");
                }

            }
        }
    }



    void Defend() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (MoveToThreatenedNode(currentArmy)) {
                //print("moving, not ready to execute");
                moreToDoInPhase = true;
                //print("Move performed, waiting");
                return;
            }
        }
    }

    void Attack() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (AttackNearbyThreat(currentArmy)) {
                //print("Attack performed, waiting");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
    }





    void BuyUnits() {
        //print("army count: "+armies.Count);
        bool moneyToSpend = true;
        while (moneyToSpend) {
            //print("has " + GetComponent<Player>().money + " money to spend");
            float greatestPowerDifference = 9999;
            GameObject weakestArmy = armies[0];
            for (int i = 0; i < armies.Count; i++) {
                GameObject armyNode = armies[i].GetComponent<Army>().currentNode;
                float powerDifference = armies[i].GetComponent<Army>().GetOffensivePower() - armyNode.GetComponent<Node>().GetThreatToNode();
                if (powerDifference < greatestPowerDifference) {
                    weakestArmy = armies[i];
                    greatestPowerDifference = powerDifference;
                }
            }
            if (weakestArmy.GetComponent<Army>().HasOpenPosition()) {
                //print("weakest has opening");
                UnitPos position = weakestArmy.GetComponent<Army>().GetOpenPosition();
                MapUnit unit = GetComponent<Player>().unitBlueprints[3];
                weakestArmy.GetComponent<Army>().BuyUnit(position, unit);
                if (GetComponent<Player>().money < unit.moneyCost) moneyToSpend = false;
            }
            else moneyToSpend = false;
        }
    }

    bool MoveToThreatenedNode(GameObject army) {

        //print("checking possible threats");
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        Tools.DeepCopyGameObjectList(ownedNodes);
        List<GameObject> nodesByThreat = Tools.DeepCopyGameObjectList(ownedNodes);
        nodesByThreat.Sort(Tools.SortByThreat);

        int index = 0;
        while (index < nodesByThreat.Count) {
            GameObject node = nodesByThreat[index];
            if (node.GetComponent<Node>().occupant != null) {
                nodesByThreat.Remove(node);
                index--;
            }
            if (nodesByThreat.Contains(node) && node.GetComponent<Node>().GetThreatToNode() <= 0) {
                nodesByThreat.Remove(node);
                index--;
            }
            index++;
        }

        for(int i=0; i< nodesByThreat.Count; i++) {
            print("nodesByThreat: "+ nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
        }

        GameObject armyNode = army.GetComponent<Army>().currentNode;
        List<GameObject> nodesInRange = new List<GameObject>();
        nodesInRange = GetNodesInRange(nodesInRange, armyNode, army.GetComponent<Army>().movesLeft);

        GameObject reachableThreatenedNode = null;
        int j = nodesByThreat.Count - 1;
        // Search nodes by threat descending, choose a reachable node
        while (reachableThreatenedNode == null && j >= 0) {
            GameObject node = nodesByThreat[j];
            if (nodesInRange.Contains(node)) {
                reachableThreatenedNode = node;
            }
            j--;
        }

        if (reachableThreatenedNode != null && reachableThreatenedNode != armyNode && MoreThreatenedThan(reachableThreatenedNode, armyNode)) {
            print("found someone worth protecting :')");
            readyToExecute = false;
            GetComponent<Player>().attackNode(army, reachableThreatenedNode);
            return true;
        }

        return false;
    }

    bool MoreThreatenedThan(GameObject node1, GameObject node2) {
        //print("Other threat: " + node1.GetComponent<Node>().GetThreatToNode());
        //print("Threat to me: " + node2.GetComponent<Node>().GetThreatIgnoringCurrentArmy());
        if (node1.GetComponent<Node>().GetThreatToNode() > node2.GetComponent<Node>().GetThreatIgnoringCurrentArmy()) return true;
        return false;
    }

    bool AttackNearbyThreat(GameObject army) {
        //print("checking attacks");
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        GameObject target = armyNode.GetComponent<Node>().GetGreatestThreat();
        if (target != null) {
            //print("target exists");
            //print("Should I? " + ShouldAttack(army, target));
        }
        if (target != null && ShouldAttack(army, target)) {
            //print("ATTACK!");
            GetComponent<Player>().attackNode(army, target);
            return true;
        }
        return false;
    }

    bool ShouldAttack(GameObject attackingArmy, GameObject defendingNode) {
        //print("attack power: " + attackingArmy.GetComponent<Army>().GetOffensivePower());
        //print("defend power: " + defendingNode.GetComponent<Node>().GetDefensivePower());
        if (attackingArmy.GetComponent<Army>().GetOffensivePower() >= 1.2 * defendingNode.GetComponent<Node>().GetDefensivePower()) {
            return true;
        }
        return false;
    }


    void TestIDS() {
        for (int k = 0; k < 1; k++) {
            for (int i = 0; i < armies.Count; i++) {
                // For all armies
                GameObject army = armies[i];
                GameObject armyNode = army.GetComponent<Army>().currentNode;
                GameObject randomNeighbour = armyNode.GetComponent<Node>().GetRandomNeighbour2();
                List<GameObject> pathway = armyNode.GetComponent<Node>().GetPathTo(randomNeighbour);
                //print("Pathway to random neighbour: " + pathway);
                for (int j = 0; j < pathway.Count; j++) {
                    //print("Path includes: " + pathway[j]);
                }
            }
        }
    }

    List<GameObject> GetNodesInRange(List<GameObject> nodes, GameObject node, int range) {
        if (!nodes.Contains(node)) nodes.Add(node);
        if (range > 0) {
            List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
            for (int i = 0; i < neighbours.Count; i++) {
                GameObject neighbour = neighbours[i];
                if (neighbour.GetComponent<Node>().faction == faction && !neighbour.GetComponent<Node>().occupied) GetNodesInRange(nodes, neighbour, range - 1);
            }
        }
        return nodes;
    }

    float ThreatToNode(GameObject node) {
        float threat = 0;
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction != faction) {
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetOffensivePower());
            }
        }
        return threat;
    }

    public void StartTurn() {
        turnPhase = TurnPhase.DefendAgainstThreats;
        readyToExecute = true;
    }
    bool readyForNextPhase() {
        if (!moreToDoInPhase && Army.readyToMove) return true;
        return false;
    }



    void Scout() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (currentArmy.GetComponent<Army>().movesLeft > 0 && NearbyUnscoutedArmy(currentArmy)) {
                //print("Attack performed, waiting");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
    }

    bool NearbyUnscoutedArmy(GameObject army) {
        GameObject node = army.GetComponent<Army>().currentNode;
        for (int i = 0; i < node.GetComponent<Node>().neighbours.Count; i++) {
            GameObject neighbourNode = node.GetComponent<Node>().neighbours[i];
            GameObject neighbouringArmy = neighbourNode.GetComponent<Node>().occupant;
            if (neighbouringArmy != null) {
                if (!enemyArmyInfo.ContainsKey(neighbouringArmy)  || (enemyArmyInfo.ContainsKey(neighbouringArmy) && enemyArmyInfo[neighbouringArmy].timeSinceScout >= 2)) {
                    print("Go Scout: " + neighbourNode.name);
                    readyToExecute = false;
                    RememberArmy(neighbouringArmy);
                    GetComponent<Player>().attackNode(army, neighbourNode);
                    return true;
                }
            }
        }
        return false;
    }

    public void RememberArmy(GameObject enemyArmy) {
        ArmyInfo newInfo = new ArmyInfo(0, enemyArmy.GetComponent<Army>().GetOffensivePower());
        if (enemyArmyInfo.ContainsKey(enemyArmy)) {
            enemyArmyInfo[enemyArmy] = newInfo;
        }
        else {
            enemyArmyInfo.Add(enemyArmy, newInfo);
        }
    }


    /*

    void ExecuteIntent() {
        print("Executing Intent");
        readyToExecute = false;
        Intent intent = intentQueue[0];
        switch (intent.type) {
            case "Attack":
                print("execute attack");
                GetComponent<Player>().attackNode(intent.army, intent.node);
                break;
            case "Move":
                print("execute move");
                GetComponent<Player>().attackNode(intent.army, intent.node);
                break;
        }
        intentQueue.RemoveAt(0);

    }

    void CreateIntent(string intType, GameObject army, GameObject node) {
        print("Creating Intent");
        print("Intent Name: " + intType);
        print("Intent Army: " + army);
        print("Intent Target: " + node);
        Intent intent = new Intent();
        intent.type = intType;
        intent.army = army;
        intent.node = node;
        intentQueue.Add(intent);
    }
    */

    /*
void MoveToThreatened(List<GameObject> nodesByThreat) {
    //TestIDS();
    for (int i = 0; i < nodesByThreat.Count; i++) {
        //print(nodesByThreat[i] + ": " + nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
    }

    for (int i = 0; i < armies.Count; i++) {
        // For all armies
        GameObject army = armies[i];
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        List<GameObject> nodesInRange = new List<GameObject>();
        nodesInRange = GetNodesInRange(nodesInRange, armyNode, army.GetComponent<Army>().movesLeft);

        GameObject reachableThreatenedNode = null;
        int j = nodesByThreat.Count - 1;
        // Search nodes by threat descending, choose a reachable node
        while (reachableThreatenedNode == null) {
            GameObject node = nodesByThreat[j];
            if (nodesInRange.Contains(node)) {
                reachableThreatenedNode = node;
            }
            j--;
        }
        // If reachable node is found, attack that node
        if (reachableThreatenedNode) {
            //print("Found and moving to: " + reachableThreatenedNode);
            CreateIntent("Move", army, reachableThreatenedNode);
            //GetComponent<Player>().attackNode(army, reachableThreatenedNode);
            //print("Moves left: " + army.GetComponent<Army>().movesLeft);
            nodesByThreat.Remove(reachableThreatenedNode);
        }
    }
}

void AttackLargestThreat() {
    for (int i = 0; i < armies.Count; i++) {
        GameObject army = armies[i];
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        GameObject target = armyNode.GetComponent<Node>().GetGreatestThreat();
        if (target!= null) {
            CreateIntent("Attack", army, target);
            //GetComponent<Player>().attackNode(army, target);
        }
    }
}
*/

    // Deprecated
    /*
    GameObject GetMostThreatenedNeighbour(GameObject startNode, int range) {
        GameObject mostThreatened = startNode;
        int threat = ThreatToNode(startNode);
        if (range > 0) {
            for (int i = 0; i < startNode.GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = startNode.GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().faction == faction) {

                }
            }
        }
    }
    
    void CompileOptionsStage() {
        print("are you even alive? 1");
        BuyUnits();

        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);
        MoveToThreatened(nodesByThreat);

        AttackLargestThreat();
        turnStage = 1;
    }
    void ExecutionStage() {
        print("are you even alive? 2");
        if (intentQueue.Count != 0 && readyToExecute) {
            ExecuteIntent();
        }

        if (intentQueue.Count == 0) turnStage = 2;
    }
    */
}
