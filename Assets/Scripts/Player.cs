using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static GameObject highlightFog;

    public List<GameObject> Armies = new List<GameObject>();
    public bool isArmySelected = false;
    public static GameObject selectedArmy;
    public static GameObject nodeClicked;
    public static GameObject armyClicked;

    // Start is called before the first frame update
    void Start(){
        highlightFog = GameObject.Find("/Highlight");
        highlightFog.transform.position = new Vector3(0, 0, 0);
        highlightFog.SetActive(false);
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
        for (int i = 0; i < Armies.Count; i++) {
            GameObject army = Armies[i];
            if (army.GetComponent<Army>().selected) {
                isArmySelected = true;
                selectedArmy = army;
            }
        }
        if (!isArmySelected) selectedArmy = null;

        highlightFog.SetActive(isArmySelected);
    }

    public void ImplementClicks() {
        print("Node clicked: " + nodeClicked);
        if (armyClicked && armyClicked.GetComponent<Army>().currentNode.GetComponent<Node>().highlighted) {
            nodeClicked = armyClicked.GetComponent<Army>().currentNode;
        }
        if (nodeClicked && nodeClicked.GetComponent<Node>().highlighted && isArmySelected) {
            print("Moving army");
            attackNode(selectedArmy, nodeClicked);
            selectedArmy.GetComponent<Army>().Deselect();
            if (armyClicked) print("also moving other army");
        }
        else if (armyClicked) {
            if (!selectedArmy) {
                armyClicked.GetComponent<Army>().Select();
                print("new army selected");
            }
            else {
                print("no army selected, deselecting");
                selectedArmy.GetComponent<Army>().Deselect();
                if (armyClicked != selectedArmy) {
                    print("different army, selecting new one");
                    armyClicked.GetComponent<Army>().Select();
                    selectedArmy = armyClicked;
                }
            }
        }
        else if (selectedArmy && Input.GetMouseButtonDown(0)) {
            print("clicked on anything else, deselecting");
            selectedArmy.GetComponent<Army>().Deselect();
        }
        armyClicked = null;
        nodeClicked = null;
    }

    public void attackNode(GameObject army, GameObject node) {
        if (node.GetComponent<Node>().occupiable) {
            if (!node.GetComponent<Node>().occupied) {
                print("moving in freely");
                army.GetComponent<Army>().MoveToNode(node);
            }
            else {
                print("node occupied");
                GameObject otherArmy = node.GetComponent<Node>().occupant;
                if (army.GetComponent<Army>().race != army.GetComponent<Army>().race) {
                    print("attack!");
                }
                else {
                    print("change places");
                    switchNodes(army, army.GetComponent<Army>().currentNode, otherArmy, node);
                    //otherArmy.GetComponent<Army>().MoveToNode(army.GetComponent<Army>().currentNode);
                    //army.GetComponent<Army>().MoveToNode(node);

                }
            }
        }
        //army.GetComponent<Army>().movesLeft -= 1;
    }

    public void switchNodes(GameObject army1, GameObject node1, GameObject army2, GameObject node2) {
        army1.GetComponent<Army>().MoveToNode(node2);
        army2.GetComponent<Army>().MoveToNode(node1);
        node2.GetComponent<Node>().occupied = true;
        node2.GetComponent<Node>().occupant = army1;
    }

}
