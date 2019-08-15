﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<MapUnit> unitBlueprints = new List<MapUnit>();
    public List<Ritual> ritualBlueprints = new List<Ritual>();

    public static GameObject human;
    public static GameObject selectedArmy;
    public static GameObject nodeClicked;
    public static GameObject armyLeftClicked;
    public static GameObject armyRightClicked;
    public static GameObject nodeMenu;
    public static GameObject battleMenu;
    public static int menuOpen = 0;

    public RaceTraits raceTraits;
    public Race race = Race.Noumenon;
    public int money = 20;
    public int zeal = 0;
    public bool isArmySelected = false;

    public void Awake() {
        if (name == "Human") human = gameObject;
    }

    // Start is called before the first frame update
    void Start(){
        SetupRaceTraits();
    }

    // Update is called once per frame
    void Update(){
        //print(TurnManager.currentPlayer);
        if (gameObject == TurnManager.currentPlayer) {
            CheckSelected();
        }
    }

    private void LateUpdate() {
        if (gameObject == TurnManager.currentPlayer) {
            ImplementClicks();
        }
    }


    public void Startup() {
        print("initializing members");
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.GetComponent<Army>()) {
                armies.Add(child);
            }
        }
        //armies.Add(GameObject.Find("/Army Blue"));
        //armies.Add(GameObject.Find("/Army Red"));
        nodeMenu = GameObject.Find("/Node Menu");
        battleMenu = GameObject.Find("/Battle Menu");

        for (int i = 0; i < NodeManager.nodes.Count; i++) {
            GameObject node = NodeManager.nodes[i];
            if (node.GetComponent<Node>().owner == race) ownedNodes.Add(node);
        }
        //SetupRaceTraits();
    }

    void SetupRaceTraits() {
        raceTraits = GameObject.Find("/Race Manager").GetComponent<RaceManager>().GetRaceTraits(race);
    }

    void CheckSelected() {
        isArmySelected = false;
        for (int i = 0; i < armies.Count; i++) {
            if (armies[i] != null) {
                GameObject army = armies[i];
                if (army.GetComponent<Army>().selected) {
                    isArmySelected = true;
                    selectedArmy = army;
                }
            }
        }
        if (!isArmySelected) selectedArmy = null;

        NodeManager.highlightFog.SetActive(isArmySelected);
    }

    public void ImplementClicks() {
        //print("Node clicked: " + nodeClicked);
        // Node clicked again, Deselect
        if (armyLeftClicked && armyLeftClicked.GetComponent<Army>().currentNode.GetComponent<Node>().highlighted) {
            nodeClicked = armyLeftClicked.GetComponent<Army>().currentNode;
        }
        // Army is selected and clicked node is highlighted, Attack 
        if (nodeClicked && nodeClicked.GetComponent<Node>().highlighted && isArmySelected) {
            //print("Moving army");
            attackNode(selectedArmy, nodeClicked);
            selectedArmy.GetComponent<Army>().Deselect();
        }
        // Different army clicked, Select new army
        else if (armyLeftClicked) {
            // No army Selected, Select army
            if (!selectedArmy) {
                if (armyLeftClicked.GetComponent<Army>().movesLeft > 0) {
                    //print("A");
                    armyLeftClicked.GetComponent<Army>().Select();
                }
            }
            else {
                //print("no army selected, deselecting");
                selectedArmy.GetComponent<Army>().Deselect();
                if (armyLeftClicked != selectedArmy && selectedArmy.GetComponent<Army>().movesLeft > 0) {
                    //print("different army, selecting new one");
                    //print("B");
                    armyLeftClicked.GetComponent<Army>().Select();
                    selectedArmy = armyLeftClicked;
                }
            }
        }
        else if (selectedArmy && Input.GetMouseButtonDown(0)) {
            //print("clicked on anything else, deselecting");
            selectedArmy.GetComponent<Army>().Deselect();
        }

        //Right click
        if (Input.GetMouseButtonDown(1)) {
            if (!nodeMenu.GetComponent<NodeMenu>().open && menuOpen == 0) {
                if (nodeClicked) nodeMenu.GetComponent<NodeMenu>().EnterMenu(nodeClicked);
                if (armyRightClicked) nodeMenu.GetComponent<NodeMenu>().EnterMenu(armyRightClicked.GetComponent<Army>().currentNode);
            }
            else if (menuOpen == 1) {
                nodeMenu.GetComponent<NodeMenu>().ExitMenu();
            }
        }

        armyLeftClicked = null;
        armyRightClicked = null;
        nodeClicked = null;
    }

    public void attackNode(GameObject army, GameObject node) {
        army.GetComponent<Army>().movesLeft--;

        GameObject finalNode = node;
        // If army isn't next to target, attack one step closer until adjacent
        while (!army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours.Contains(node) && army.GetComponent<Army>().currentNode != node) {
            //print("not adjacent to target yet");
            List<GameObject> moveList = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetPathTo(node);
            moveList.RemoveAt(0);
            node = moveList[0];
            attackNode(army, node);
        }
        // If army is next to target, go there        
        if (army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours.Contains(finalNode)) {
            //print("adjacent to target");
            if (finalNode.GetComponent<Node>().occupiable) {
                if (!finalNode.GetComponent<Node>().occupied) {
                    //print("moving in freely");
                    army.GetComponent<Army>().MoveToNode(finalNode);
                }
                else {
                    print("node occupied");
                    GameObject otherArmy = finalNode.GetComponent<Node>().occupant;
                    if (army.GetComponent<Army>().race != otherArmy.GetComponent<Army>().race) {
                        Invade(army, otherArmy);
                        //print("attack!");
                    }
                    else {
                        //print("change places");
                        SwitchNodes(army, army.GetComponent<Army>().currentNode, otherArmy, finalNode);
                    }
                }
            }
            //army.GetComponent<Army>().movesLeft -= 1;
        }
    }

    public void Invade(GameObject attackingArmy, GameObject defendingArmy) {
        menuOpen = 1;
        battleMenu.GetComponent<BattleMenu>().EnterMenu();
        GameObject attackArmyMenu = Tools.GetChildNamed(battleMenu, "Attacking Army Menu");
        GameObject defendArmyMenu = Tools.GetChildNamed(battleMenu, "Defending Army Menu");
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendingArmy);
        battleMenu.GetComponent<BattleMenu>().SetupBattle(attackingArmy, defendingArmy, defendingArmy.GetComponent<Army>().currentNode);
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendingArmy);
    }

    public void SwitchNodes(GameObject army1, GameObject node1, GameObject army2, GameObject node2) {
        army1.GetComponent<Army>().MoveToNode(node2);
        army2.GetComponent<Army>().MoveToNode(node1);
        node2.GetComponent<Node>().occupied = true;
        node2.GetComponent<Node>().occupant = army1;
    }

    public void RemoveNode(GameObject node) {
        ownedNodes.Remove(node);
    }
    public void AddNode(GameObject node) {
        ownedNodes.Add(node);
    }

    public bool BuyRitual(Ritual ritual) {
        if (zeal >= ritual.zealCost) {
            zeal -= ritual.zealCost;
            NodeMenu.currentNode.GetComponent<Node>().plannedRitual = Tools.DeepCopyRitual(ritual);
            return true;
        }
        return false;
    }

    public void StartTurn() {
        print("Start Turn!");
        money += GetMoneyIncome();
        zeal += GetZealIncome();
        RestUnits();
        UpdateNodes();
        //print("Number 6 = " + raceTraits.UnitFunction(3));
    }

    void UpdateNodes() {
        for(int i = 0; i< ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            node.GetComponent<Node>().UpdateRitual();
        }
    }

    public int GetMoneyIncome() {
        int income = 0;
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            income += node.GetComponent<Node>().GetNodeMoneyIncome();
        }
        return income;
    }

    public int GetZealIncome() {
        int income = 0;
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            income += node.GetComponent<Node>().GetNodeZealIncome();
        }
        return income;
    }

    private void RestUnits() {
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            army.GetComponent<Army>().Refresh();
            for (int j = 0; j < army.GetComponent<Army>().units.Length; j++) {
                if (army.GetComponent<Army>().units[j] != null) {
                    MapUnit unit = army.GetComponent<Army>().units[j];
                    unit.Refresh();
                }
            }
            
        }
    }

}
