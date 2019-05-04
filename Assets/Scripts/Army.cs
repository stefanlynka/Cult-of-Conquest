using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public bool selected = false;
    public static GameObject highlightFog;

    public Race race;
    public GameObject currentNode;
    public GameObject map;
    public bool mouseOverArmy = false;
    public int maxMoves = 2;
    public int movesLeft = 2;

    public MapUnit[] backRow = new MapUnit[5];
    public MapUnit[] frontRow = new MapUnit[5];

    // Start is called before the first frame update
    void Start(){
        map = GameObject.Find("/Map");
        SetStartingUnits();

    }

    // Update is called once per frame
    void Update(){
        Inputs();
    }

    private void OnMouseEnter() {
        mouseOverArmy = true;
    }

    private void OnMouseExit() {
        mouseOverArmy = false;
    }

    void SetStartingUnits() {
        backRow[0] = new MapUnit();
        backRow[0].name = "commander";
        backRow[1] = new MapUnit();
        frontRow[2] = new MapUnit();
    }

    public void HighlightNodes(GameObject node, int distance) {
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupiable) {
                neighbour.GetComponent<Node>().Highlight();
                if (distance > 1 && neighbour.GetComponent<Node>().owner == race) {
                    HighlightNodes(neighbour, distance - 1);
                }
            }
        }
    }
    public void Inputs() {
        if (Input.GetMouseButtonDown(0)) {
        }
    }
    /*
    private void OnMouseDown() {
        Player.armyClicked = gameObject;
    }
    */
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Player.armyLeftClicked = gameObject;
        }
        if (Input.GetMouseButtonDown(1)) {
            Player.armyRightClicked = gameObject;
        }
    }

    public void UnhighlightNodes() {
        map.GetComponent<NodeManager>().UnhighlightNodes();
    }
    public void Select() {
        print("selected");
        selected = true;
        transform.localScale *= 2;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -20);
        HighlightNodes(currentNode, movesLeft);
    }
    public void Deselect() {
        print("deselected");
        selected = false;
        transform.localScale *= 0.5f;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        UnhighlightNodes();
    }
    public void MoveToNode(GameObject destination) {
        currentNode.GetComponent<Node>().occupied = false;
        currentNode.GetComponent<Node>().occupant = null;
        transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        currentNode = destination;
        destination.GetComponent<Node>().owner = race;
        destination.GetComponent<Node>().occupied = true;
        destination.GetComponent<Node>().occupant = gameObject;
    }
}

public enum Race {
    Noumenon,
    Dukkha,
    Paratrophs,
    Unmar,
    Eidalons,
    Carnot,
    Independent,
    None
}