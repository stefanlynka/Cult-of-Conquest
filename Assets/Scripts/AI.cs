using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour{

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    GameObject unitShop;

    Race race;
    public int turnStage = 0;


    // Start is called before the first frame update
    void Start(){
        CollectInformation();
    }

    // Update is called once per frame
    void Update(){
        if (TurnManager.currentPlayer == gameObject && turnStage == 0) {
            CompileOptions();
        }
        else if (TurnManager.currentPlayer != gameObject) {
            turnStage = 0;
        }
    }

    void CollectInformation() {
        armies = GetComponent<Player>().armies;
        ownedNodes = GetComponent<Player>().ownedNodes;
        race = GetComponent<Player>().race;
        //unitShop = GameObject.Find("")
    }

    void CompileOptions() {
        turnStage = 1;

        BuyUnits();

        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);
        MoveToThreatened(nodesByThreat);

        AttackLargestThreat();

    }

    void BuyUnits() {
        bool moneyToSpend = true;
        while (moneyToSpend) {
            print("has " + GetComponent<Player>().money + " money to spend");
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
                print("weakest has opening");
                UnitPos position = weakestArmy.GetComponent<Army>().GetOpenPosition();
                MapUnit unit = GetComponent<Player>().unitBlueprints[3];
                weakestArmy.GetComponent<Army>().BuyUnit(position, unit);
                if (GetComponent<Player>().money < unit.moneyCost) moneyToSpend = false;
            }
            else moneyToSpend = false;
        }
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
                GetComponent<Player>().attackNode(army, reachableThreatenedNode);
                //print("Moves left: " + army.GetComponent<Army>().movesLeft);
            }
        }
    }

    void AttackLargestThreat() {
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            GameObject target = armyNode.GetComponent<Node>().GetGreatestThreat();
            if (target!= null) {
                GetComponent<Player>().attackNode(army, target);
            }
        }
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
                if (neighbour.GetComponent<Node>().owner == race && !neighbour.GetComponent<Node>().occupied) GetNodesInRange(nodes, neighbour, range - 1);
            }
        }
        return nodes;
    }

    int ThreatToNode(GameObject node) {
        int threat = 0;
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().owner != race) {
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetPower());
            }
        }
        return threat;
    }
    /*
    GameObject GetMostThreatenedNeighbour(GameObject startNode, int range) {
        GameObject mostThreatened = startNode;
        int threat = ThreatToNode(startNode);
        if (range > 0) {
            for (int i = 0; i < startNode.GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = startNode.GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().owner == race) {

                }
            }
        }
    }
    */
    
}
