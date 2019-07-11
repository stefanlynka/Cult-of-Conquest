﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public bool selected = false;
    public static GameObject highlightFog;

    public Race race;
    public GameObject currentNode;
    public GameObject map;
    public GameObject owner;
    public bool mouseOverArmy = false;
    public int maxMoves = 2;
    public int movesLeft = 2;

    public MapUnit[] units = new MapUnit[8];
    public MapUnit[] backRow = new MapUnit[4];
    public MapUnit[] frontRow = new MapUnit[4];

    // Start is called before the first frame update
    void Start(){
        map = GameObject.Find("/Node Manager");
        SetStartingUnits();

    }

    // Update is called once per frame
    void Update(){
    }

    private void OnMouseEnter() {
        mouseOverArmy = true;
    }

    private void OnMouseExit() {
        mouseOverArmy = false;
    }
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && TurnManager.currentPlayer.GetComponent<Player>().race==race) {
            Player.armyLeftClicked = gameObject;
        }
        if (Input.GetMouseButtonDown(1) && TurnManager.currentPlayer.GetComponent<Player>().race == race) {
            Player.armyRightClicked = gameObject;
        }
    }

    void SetStartingUnits() {
        owner = TurnManager.human;
        GameObject playerList = GameObject.Find("/Players");
        owner = transform.parent.gameObject;
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

    public void UnhighlightNodes() {
        map.GetComponent<NodeManager>().UnhighlightNodes();
    }
    public void Select() {
        print("selected"+gameObject);
        selected = true;
        transform.localScale *= 2;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -20);
        HighlightNodes(currentNode, movesLeft);
    }
    public void Deselect() {
        print("deselected"+gameObject);
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

    public void BuyUnit(UnitPos unitPos, MapUnit unit) {
        print("trying to buy unit");
        if (unit.moneyCost <= owner.GetComponent<Player>().money && unit.zealCost <= owner.GetComponent<Player>().zeal) {
            owner.GetComponent<Player>().money -= unit.moneyCost;
            owner.GetComponent<Player>().zeal -= unit.zealCost;
            AddUnit(unitPos.position, unitPos.frontRow, unit);
        }
        else print("not enough money");
    }

    public void AddUnit(int index, bool fRow, MapUnit unit) {
        print("adding unit");
        MapUnit newUnit = unit.DeepCopy();
        if (fRow) frontRow[index] = newUnit;
        else backRow[index] = newUnit;
        if (fRow) index += 4;
        units[index] = newUnit;
    }

    public void Defeated() {
        print("defeated");
        currentNode.GetComponent<Node>().occupant = null;
        currentNode.GetComponent<Node>().occupied = false;
        owner.GetComponent<Player>().armies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void Refresh() {
        movesLeft = maxMoves;
    }

    public int GetPower() {
        int power = 0;
        for(int i = 0; i < units.Length; i++) {
            if (units[i] != null) {
                MapUnit unit = units[i];
                power += unit.power;
                //print("found some power: " + unit.power);
            }
        }

        return power;
    }

    public bool HasOpenPosition() {
        for (int i = 0 ; i < units.Length; i++) {
            if (units[i] == null) return true;
        }
        print("no openings");
        return false;
    }

    public UnitPos GetOpenPosition() {
        for (int i = 0; i < frontRow.Length; i++) {
            if (frontRow[i] == null) {
                return new UnitPos(i, true);
            }
        }
        for (int i = 0; i < backRow.Length; i++) {
            if (backRow[i] == null) {
                return new UnitPos(i, false);
            }
        }
        return new UnitPos(0, true);
    }
}


