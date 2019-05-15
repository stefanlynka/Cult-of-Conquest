using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static GameObject highlightFog;

    public List<GameObject> armies = new List<GameObject>();
    public bool isArmySelected = false;
    public static GameObject selectedArmy;
    public static GameObject nodeClicked;
    public static GameObject armyLeftClicked;
    public static GameObject armyRightClicked;
    public static GameObject nodeMenu;
    public static GameObject battleMenu;
    public static bool menuOpen = false;

    public static int money = 100;
    public static int zeal = 0;

    // Start is called before the first frame update
    void Start(){
        highlightFog = GameObject.Find("/Highlight");
        highlightFog.transform.position = new Vector3(0, 0, 10);
        highlightFog.SetActive(false);
        armies.Add(GameObject.Find("/Army Blue"));
        armies.Add(GameObject.Find("/Army Red"));
        nodeMenu = GameObject.Find("/Node Menu");
        battleMenu = GameObject.Find("/Battle Menu");
    }

    // Update is called once per frame
    void Update(){
        CheckSelected();
    }

    private void LateUpdate() {
        ImplementClicks();
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

        highlightFog.SetActive(isArmySelected);
    }

    public void ImplementClicks() {
        //print("Node clicked: " + nodeClicked);
        if (armyLeftClicked && armyLeftClicked.GetComponent<Army>().currentNode.GetComponent<Node>().highlighted) {
            nodeClicked = armyLeftClicked.GetComponent<Army>().currentNode;
        }
        if (nodeClicked && nodeClicked.GetComponent<Node>().highlighted && isArmySelected) {
            print("Moving army");
            attackNode(selectedArmy, nodeClicked);
            selectedArmy.GetComponent<Army>().Deselect();
            //if (armyClicked) print("also moving other army");
        }
        else if (armyLeftClicked) {
            if (!selectedArmy) {
                armyLeftClicked.GetComponent<Army>().Select();
                //print("new army selected");
            }
            else {
                //print("no army selected, deselecting");
                selectedArmy.GetComponent<Army>().Deselect();
                if (armyLeftClicked != selectedArmy) {
                    //print("different army, selecting new one");
                    armyLeftClicked.GetComponent<Army>().Select();
                    selectedArmy = armyLeftClicked;
                }
            }
        }
        else if (selectedArmy && Input.GetMouseButtonDown(0)) {
            //print("clicked on anything else, deselecting");
            selectedArmy.GetComponent<Army>().Deselect();
        }

        if (Input.GetMouseButtonDown(1)) {
            if (!nodeMenu.GetComponent<NodeMenu>().open && !menuOpen) {
                if (nodeClicked) nodeMenu.GetComponent<NodeMenu>().EnterMenu(nodeClicked);
                if (armyRightClicked) nodeMenu.GetComponent<NodeMenu>().EnterMenu(armyRightClicked.GetComponent<Army>().currentNode);
            }
            else if (!menuOpen) {
                nodeMenu.GetComponent<NodeMenu>().ExitMenu();
            }
        }

        armyLeftClicked = null;
        armyRightClicked = null;
        nodeClicked = null;
    }

    public void attackNode(GameObject army, GameObject node) {
        if (node.GetComponent<Node>().occupiable) {
            if (!node.GetComponent<Node>().occupied) {
                //print("moving in freely");
                army.GetComponent<Army>().MoveToNode(node);
            }
            else {
                //print("node occupied");
                GameObject otherArmy = node.GetComponent<Node>().occupant;
                if (army.GetComponent<Army>().race != otherArmy.GetComponent<Army>().race) {
                    Invade(army, otherArmy);
                    print("attack!");
                }
                else {
                    print("change places");
                    switchNodes(army, army.GetComponent<Army>().currentNode, otherArmy, node);
                }
            }
        }
        //army.GetComponent<Army>().movesLeft -= 1;
    }

    public void Invade(GameObject attackingArmy, GameObject defendingArmy) {
        menuOpen = true;
        battleMenu.GetComponent<BattleMenu>().EnterMenu();
        GameObject attackArmyMenu = Tools.GetChildNamed(battleMenu, "Attacking Army Menu");
        GameObject defendArmyMenu = Tools.GetChildNamed(battleMenu, "Defending Army Menu");
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendingArmy);
        battleMenu.GetComponent<BattleMenu>().SetupBattle(attackingArmy, defendingArmy, nodeClicked);
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendingArmy);
    }

    public void switchNodes(GameObject army1, GameObject node1, GameObject army2, GameObject node2) {
        army1.GetComponent<Army>().MoveToNode(node2);
        army2.GetComponent<Army>().MoveToNode(node1);
        node2.GetComponent<Node>().occupied = true;
        node2.GetComponent<Node>().occupant = army1;
    }

}
