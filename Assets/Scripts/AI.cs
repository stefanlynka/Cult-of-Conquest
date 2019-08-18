using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour{

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<Intent> intentQueue = new List<Intent>();
    GameObject unitShop;

    Faction faction;
    public int turnStage = 9;
    public bool readyToExecute = true;


    // Start is called before the first frame update
    void Start(){
        CollectInformation();
    }

    // Update is called once per frame
    void Update(){
        if (TurnManager.currentPlayer == gameObject){
            if (turnStage == 0) {
                CompileOptionsStage();
            }
            if (turnStage == 1) {
                ExecutionStage();
            }
            if (turnStage == 9 && readyToExecute) {
                RunAI();
            }
        }
        else if (TurnManager.currentPlayer != gameObject) {
            turnStage = 9;
        }
    }

    void CollectInformation() {
        armies = GetComponent<Player>().armies;
        ownedNodes = GetComponent<Player>().ownedNodes;
        faction = GetComponent<Player>().faction;
        //unitShop = GameObject.Find("")
        turnStage = 9;
    }

    void RunAI() {
        BuyUnits();
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (AttackNearbyThreat(currentArmy)) {
                //print("Attack performed, waiting");
                readyToExecute = false;
                return;
            }
            
            if (MoveToThreatenedNode(currentArmy)) {
                //print("Move performed, waiting");
                return;
            }
        }
    }

    // 
    void CompileOptionsStage() {
        BuyUnits();

        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);
        MoveToThreatened(nodesByThreat);

        AttackLargestThreat();
        turnStage = 1;
    }
    void ExecutionStage() {
        if (intentQueue.Count != 0 && readyToExecute) {
            ExecuteIntent();
        }

        if (intentQueue.Count == 0) turnStage = 2;
    }

    void BuyUnits() {
        //print("army count: "+armies.Count);
        bool moneyToSpend = true;
        while (moneyToSpend) {
            //print("has " + GetComponent<Player>().money + " money to spend");
            int greatestPowerDifference = 9999;
            GameObject weakestArmy = armies[0];
            for (int i = 0; i < armies.Count; i++) {
                GameObject armyNode = armies[i].GetComponent<Army>().currentNode;
                int powerDifference = armies[i].GetComponent<Army>().GetPower() - armyNode.GetComponent<Node>().GetThreatToNode();
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
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);

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

        if (reachableThreatenedNode && reachableThreatenedNode != armyNode) {
            GetComponent<Player>().attackNode(army, reachableThreatenedNode);
            return true;
        }

        return false;
    }

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

    bool AttackNearbyThreat(GameObject army) {
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
        if (attackingArmy.GetComponent<Army>().GetPower() >= 1.2 * defendingNode.GetComponent<Node>().GetPower()) {
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
                print("Pathway to random neighbour: " + pathway);
                for (int j = 0; j < pathway.Count; j++) {
                    print("Path includes: " + pathway[j]);
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

    int ThreatToNode(GameObject node) {
        int threat = 0;
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction != faction) {
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetPower());
            }
        }
        return threat;
    }

    void ExecuteIntent() {
        print("Executing Intent");
        readyToExecute = false;
        Intent intent = intentQueue[0];
        switch (intent.type) {
            case "Attack":
                GetComponent<Player>().attackNode(intent.army, intent.node);
                break;
            case "Move":
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
    */
    
}
