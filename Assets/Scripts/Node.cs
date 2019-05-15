using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string locationName;
    public int difficulty = 1;
    public Race homeBase = Race.None;

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
    public Race owner = Race.Independent;
    public GameObject occupant;
    public bool occupied = false;
    public bool occupiable = true;
    public bool highlighted = false;



    public string effigy = "";
    public Church church = Church.None;
    public Altar altar = Altar.None;



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
        Player.nodeClicked = gameObject;
    }
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            Player.nodeClicked = gameObject;
        }
    }
    public GameObject GetOccupant() {
        if (occupant) return occupant;
        else return null;
    }

    private void Setup() {
        if (homeBase != Race.None) {
            difficulty = 4;
        }
        else {
            GameObject army = new GameObject();
            army.AddComponent<Army>();
            army.GetComponent<Army>().race = Race.Independent;
            army.GetComponent<Army>().currentNode = gameObject;
            army.transform.position = transform.position;
            occupant = army;
            occupied = true;
            MapUnit peon = MakeNeutralUnit("peon");
            MapUnit acolyte = MakeNeutralUnit("acolyte");
            MapUnit shaman = MakeNeutralUnit("shaman");
            MapUnit prelate = MakeNeutralUnit("prelate");
            switch (difficulty) {
                case 1:
                    occupant.GetComponent<Army>().addUnit(0, true, peon);
                    occupant.GetComponent<Army>().addUnit(1, true, peon);
                    occupant.GetComponent<Army>().addUnit(2, true, peon);
                    break;
                case 2:
                    occupant.GetComponent<Army>().addUnit(0, true, peon);
                    occupant.GetComponent<Army>().addUnit(1, true, acolyte);
                    occupant.GetComponent<Army>().addUnit(2, true, acolyte);
                    break;
                case 3:
                    occupant.GetComponent<Army>().addUnit(0, true, peon);
                    occupant.GetComponent<Army>().addUnit(1, true, peon);
                    occupant.GetComponent<Army>().addUnit(2, true, acolyte);
                    occupant.GetComponent<Army>().addUnit(3, true, acolyte);
                    occupant.GetComponent<Army>().addUnit(0, false, shaman);
                    break;
                case 4:
                    occupant.GetComponent<Army>().addUnit(0, true, acolyte);
                    occupant.GetComponent<Army>().addUnit(1, true, acolyte);
                    occupant.GetComponent<Army>().addUnit(2, true, shaman);
                    occupant.GetComponent<Army>().addUnit(3, true, shaman);
                    occupant.GetComponent<Army>().addUnit(0, false, prelate);
                    occupant.GetComponent<Army>().addUnit(0, false, prelate);
                    break;
            }
            occupant.transform.parent = GameObject.Find("/Neutral Armies").transform;
        }
    }

    MapUnit MakeNeutralUnit(string unitType) {
        MapUnit unit = new MapUnit(unitType, Race.Independent, unitType);
        if (unitType == "peon") {
            unit.SetHealth(50);
            unit.damage = 10;
            unit.attackRange = 1;
            unit.attackSpeed = 60;
            unit.moneyCost = 5;
            unit.zealCost = 0;
            unit.power = 10;
        }
        if (unitType == "acolyte") {
            unit.SetHealth(100);
            unit.damage = 20;
            unit.attackRange = 1;
            unit.attackSpeed = 100;
            unit.moneyCost = 10;
            unit.zealCost = 15;
            unit.power = 15;
        }
        if (unitType == "shaman") {
            unit.SetHealth(100);
            unit.damage = 10;
            unit.attackRange = 3;
            unit.attackSpeed = 120;
            unit.moneyCost = 10;
            unit.zealCost = 1;
            unit.power = 15;
        }
        if (unitType == "prelate") {
            unit.SetHealth(200);
            unit.damage = 40;
            unit.attackRange = 2;
            unit.attackSpeed = 120;
            unit.moneyCost = 20;
            unit.zealCost = 0;
            unit.power = 25;
        }

        return unit;
    }
}

/*
switch (race) {
case Race.Noumenon:
    MakeNoumenon();
    break;
*/

