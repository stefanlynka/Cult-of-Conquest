﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour{

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();

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
    }

    void CompileOptions() {
        turnStage = 1;
        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);
        for (int i = 0; i < nodesByThreat.Count; i++) {
            print(nodesByThreat[i] + ": " + nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
        }

        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            List<GameObject> nodesInRange = new List<GameObject>();
            nodesInRange = GetNodesInRange(nodesInRange, armyNode, army.GetComponent<Army>().movesLeft);

            GameObject reachableThreatenedNode = null;
            int j = nodesByThreat.Count - 1;
            while (reachableThreatenedNode == null) {
                GameObject node = nodesByThreat[j];
                if (nodesInRange.Contains(node)) {
                    reachableThreatenedNode = node;
                }
                j--;
            }
            if (reachableThreatenedNode) {
                print("found and moving to: " + reachableThreatenedNode);
                GetComponent<Player>().attackNode(army, reachableThreatenedNode);
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
