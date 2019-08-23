﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string locationName;
    public int difficulty = 1;
    public Faction homeBase = Faction.None;

    public List<GameObject> neighbours = new List<GameObject>();
    public GameObject neighbourUpLeft;
    public GameObject neighbourUp;
    public GameObject neighbourUpRight;
    public GameObject neighbourDownLeft;
    public GameObject neighbourDown;
    public GameObject neighbourDownRight;

    public Location location;

    public int moneyIncome = 1;
    public int zealIncome = 0;
    public string nodeAttribute = "";
    public List<string> tempBonus = new List<string>();
    public Faction faction = Faction.Independent;
    public GameObject occupant;
    public GameObject owner;
    public bool occupied = false;
    public bool occupiable = true;
    public bool highlighted = false;
    public bool hidden = false;
    public int sight = 1;
    public int concealment = 1;
    public Ritual ritual;

    public string effigy = "";
    public Temple temple;
    public Altar altar;



    // Start is called before the first frame update
    void Start(){
        Setup();
    }

    // Update is called once per frame
    void Update(){
    }

    public void Highlight() {
        //print("highlighted: "+ gameObject);
        if (!highlighted) transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-10);
        highlighted = true;
    }
    public void Unhighlight() {
        if (highlighted) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        highlighted = false;
    }

    private void OnMouseDown() {
        if (RitualManager.ritualSelected && highlighted) GameObject.Find("/Ritual Menu").GetComponent<RitualManager>().SelectNode(gameObject);
        //Player.nodeClicked = gameObject;
    }
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Player.nodeLeftClicked = gameObject;
        }
        else if (Input.GetMouseButtonDown(1)) {
            Player.nodeRightClicked = gameObject;
        }
    }
    public GameObject GetOccupant() {
        if (occupant) return occupant;
        else return null;
    }

    // ITERATIVE DEEPENING SEEEEAAAAAAAARCH
    public List<GameObject> GetPathTo(GameObject node) {
        List<GameObject> path = new List<GameObject>();
        int depth = 1;
        Faction player = faction;
        bool solved = false;
        while (solved == false) {
            List<GameObject> goodPath = new List<GameObject>();
            goodPath = ExtendPathToNeighbours(node, goodPath, depth, player);
            if (goodPath != null) return goodPath;
            depth++;
            if (depth > 8) {
                //print("we couldn't find it");
                return null;
            }
        }
        return null;
    }
    // Recursive path search used in GetPathTo()
    List<GameObject> ExtendPathToNeighbours(GameObject targetNode, List<GameObject> visited, int currentDepth, Faction player) {
        visited.Add(gameObject);
        if (gameObject == targetNode) {
            //print("found the one");
            return visited;
        }
        if (currentDepth > 0) {
            currentDepth--;
            for (int i = 0; i < neighbours.Count; i++) {
                GameObject neighbour = neighbours[i];
                List<GameObject> goodPath = new List<GameObject>();
                if (neighbour.GetComponent<Node>().faction == player && !visited.Contains(neighbour)) {
                    List<GameObject> deepCopyVisited = Tools.DeepCopyGameObjectList(visited);
                    goodPath = neighbour.GetComponent<Node>().ExtendPathToNeighbours(targetNode, deepCopyVisited, currentDepth, player);
                    if (goodPath != null) {
                        visited.Insert(0,gameObject);
                        //print("part of the path");
                        return goodPath;
                    }
                }
            }
        }
        //print("not here");
        return null;
    }

    // Get a random neighbour
    public GameObject GetRandomNeighbour() {
        int rand = Random.Range(0, neighbours.Count);
        return neighbours[rand];
    }
    // Get a neighbour of a neighbour who isn't this node
    public GameObject GetRandomNeighbour2() {
        //print("looking for n2");
        int rand = Random.Range(0, neighbours.Count);
        GameObject neighbour = neighbours[rand];
        GameObject secondNeighbour = neighbour.GetComponent<Node>().GetRandomNeighbour();
        while (secondNeighbour == gameObject) secondNeighbour = neighbour.GetComponent<Node>().GetRandomNeighbour();
        //print("found second neighbour at: x:" + secondNeighbour.transform.localPosition.x + " y:" + secondNeighbour.transform.localPosition.y);
        return secondNeighbour;
    }

    public GameObject GetGreatestThreat() {
        GameObject biggestThreat = null;
        int threat = 0;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];

            // If the neighbouring node is occupied, is owned by another faction, and is the new biggest threat
            if (neighbour.GetComponent<Node>().faction != faction && threat < neighbour.GetComponent<Node>().GetPower()) {
                biggestThreat = neighbour;
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().GetPower());
            }
            /*
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction != faction && neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetPower() > threat) {
                biggestThreat = neighbour;
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetPower());
            }
            */
        }
        return biggestThreat;
    }

    public void UpdateRitual() {
        if (ritual.name != null) {
            ritual.prepTime--;
            if (ritual.prepTime == 0) {
                print("RITUAL READY!!!!!!!");
            }
            if (ritual.prepTime < 0) {
            }
        }
    }

    public void UpdateSprite() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (homeBase == Faction.None) {
            Sprite newSprite = renderer.sprite;
            switch (faction) {
                case Faction.Carnot:
                    newSprite = Resources.Load<Sprite>("Nodes/CarnotNode");
                    break;
                case Faction.Dukkha:
                    newSprite = Resources.Load<Sprite>("Nodes/DukkhaNode");
                    break;
                case Faction.Samata:
                    newSprite = Resources.Load<Sprite>("Nodes/SamataNode");
                    break;
                case Faction.Noumenon:
                    newSprite = Resources.Load<Sprite>("Nodes/NoumenonNode");
                    break;
                case Faction.Paratrophs:
                    newSprite = Resources.Load<Sprite>("Nodes/ParatrophNode");
                    break;
                case Faction.Unmar:
                    newSprite = Resources.Load<Sprite>("Nodes/UnmarNode");
                    break;
            }
            renderer.sprite = newSprite;
        }
    }

    private void Setup() {
        if (homeBase != Faction.None) {
            difficulty = 4;
            faction = homeBase;
        }
        else if (faction == Faction.Independent) {
            GameObject army = new GameObject();
            army.AddComponent<Army>();
            army.GetComponent<Army>().faction = Faction.Independent;
            army.GetComponent<Army>().currentNode = gameObject;
            army.GetComponent<Army>().owner = GameObject.Find("/Neutral Player");
            army.transform.position = transform.position;
            occupant = army;
            occupied = true;
            MapUnit peon = MakeNeutralUnit("peon");
            MapUnit acolyte = MakeNeutralUnit("acolyte");
            MapUnit shaman = MakeNeutralUnit("shaman");
            MapUnit prelate = MakeNeutralUnit("prelate");
            switch (difficulty) {
                case 1:
                    occupant.GetComponent<Army>().AddUnit(0, true, peon);
                    occupant.GetComponent<Army>().AddUnit(1, true, peon);
                    occupant.GetComponent<Army>().AddUnit(2, true, peon);
                    break;
                case 2:
                    occupant.GetComponent<Army>().AddUnit(0, true, peon);
                    occupant.GetComponent<Army>().AddUnit(1, true, acolyte);
                    occupant.GetComponent<Army>().AddUnit(2, true, acolyte);
                    break;
                case 3:
                    occupant.GetComponent<Army>().AddUnit(0, true, peon);
                    occupant.GetComponent<Army>().AddUnit(1, true, peon);
                    occupant.GetComponent<Army>().AddUnit(2, true, acolyte);
                    occupant.GetComponent<Army>().AddUnit(3, true, acolyte);
                    occupant.GetComponent<Army>().AddUnit(0, false, shaman);
                    break;
                case 4:
                    occupant.GetComponent<Army>().AddUnit(0, true, acolyte);
                    occupant.GetComponent<Army>().AddUnit(1, true, acolyte);
                    occupant.GetComponent<Army>().AddUnit(2, true, shaman);
                    occupant.GetComponent<Army>().AddUnit(3, true, shaman);
                    occupant.GetComponent<Army>().AddUnit(0, false, prelate);
                    occupant.GetComponent<Army>().AddUnit(0, false, prelate);
                    break;
            }
            occupant.transform.parent = GameObject.Find("/Neutral Armies").transform;
        }
        else if (faction != Faction.Independent && occupant != null) {
            //owner = occupant.GetComponent<Army>().owner;
        }
        UpdateSprite();
    }

    public void BuildAltar(Altar newAltar) {
        altar = newAltar.DeepCopy();
    }

    public void BuildTemple(Temple newTemple) {
        temple = newTemple.DeepCopy();
    }

    public int GetNodeMoneyIncome() {
        int income = difficulty * 5;
        if (altar != null && altar.name == AltarName.Harvest) income =  (int)Mathf.Round(income * 1.4f);
        return income;
    }

    public int GetNodeZealIncome() {
        int income = 0;
        if (altar != null && altar.name == AltarName.Devotion) income = 1;
        return income;
    }

    MapUnit MakeNeutralUnit(string unitType) {
        MapUnit unit = new MapUnit(unitType, Faction.Independent, unitType);
        if (unitType == "peon") {
            unit.SetHealth(50);
            unit.maxDamage = 10;
            unit.attackRange = 1;
            unit.attackSpeed = 60;
            unit.moneyCost = 5;
            unit.zealCost = 0;
            unit.power = 10;
        }
        if (unitType == "acolyte") {
            unit.SetHealth(100);
            unit.maxDamage = 20;
            unit.attackRange = 1;
            unit.attackSpeed = 100;
            unit.moneyCost = 10;
            unit.zealCost = 15;
            unit.power = 15;
        }
        if (unitType == "shaman") {
            unit.SetHealth(100);
            unit.maxDamage = 10;
            unit.attackRange = 3;
            unit.attackSpeed = 120;
            unit.moneyCost = 10;
            unit.zealCost = 1;
            unit.power = 15;
        }
        if (unitType == "prelate") {
            unit.SetHealth(200);
            unit.maxDamage = 40;
            unit.attackRange = 2;
            unit.attackSpeed = 120;
            unit.moneyCost = 20;
            unit.zealCost = 0;
            unit.power = 25;
        }

        return unit;
    }

    public int GetThreatToNode() {
        int threat = 0;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction != faction) {
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetPower());
            }
        }
        return threat;
    }

    public float GetSafety() {
        float friendlyNeighbours = 0;
        for(int i =0; i< neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().faction == faction) friendlyNeighbours++;
        }
        float safety = friendlyNeighbours / 6;
        return safety;
    }

    public int GetPower() {
        int power = 0;
        if (occupant != null)                                       power += occupant.GetComponent<Army>().GetPower();
        if (altar != null && altar.name == AltarName.Conflict)      power += 30;
        if (temple != null && temple.name == TempleName.Protection) power += 50;

        return power;
    }

    public void RevealNode(int sight) {
        hidden = false;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (sight >= neighbour.GetComponent<Node>().concealment) {
                neighbour.GetComponent<Node>().RevealNode(sight - neighbour.GetComponent<Node>().concealment);
            }
        }
    }
    public void SetFog() {
        Tools.GetChildNamed(gameObject, "Fog Node").SetActive(hidden);
    }

}

/*
switch (faction) {
case Faction.Noumenon:
    MakeNoumenon();
    break;
*/

