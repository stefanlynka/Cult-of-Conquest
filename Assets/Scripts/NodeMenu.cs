using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMenu : MonoBehaviour
{
    GameObject currentNode;
    GameObject[] backRowSpaces = new GameObject[5];
    GameObject[] frontRowSpaces = new GameObject[5];

    public bool open = false;

    // Start is called before the first frame update
    void Start(){
        FindUnitSpaces();
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void EnterMenu(GameObject node) {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -10));
        print("node = " + node);
        print("occupant = "+node.GetComponent<Node>().GetOccupant());
        GameObject army = node.GetComponent<Node>().GetOccupant();
        if (army) LoadArmy(army);
        open = true;
    }

    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 11, -10));
        CleanUnitSpaces();
        open = false;
    }

    void LoadArmy(GameObject army) {
        MapUnit[] backUnits = army.GetComponent<Army>().backRow;
        MapUnit[] frontUnits = army.GetComponent<Army>().frontRow;
        for (int i = 0; i < backUnits.Length; i++) {
            if (backUnits[i] != null) {
                print("got a back");
                FillUnitSpace(backRowSpaces[i], backUnits[i]);
            }
        }
        for (int i = 0; i < frontUnits.Length; i++) {
            if (frontUnits[i] != null) {
                print("got a front");
                FillUnitSpace(frontRowSpaces[i], frontUnits[i]);
            }
        }
    }

    void FindUnitSpaces() {
        GameObject armyMenu = Tools.GetChildNamed(gameObject, "Army Menu");
        GameObject unitRows = Tools.GetChildNamed(armyMenu, "Unit Rows");
        GameObject backRow = Tools.GetChildNamed(unitRows, "Back Row");
        GameObject frontRow = Tools.GetChildNamed(unitRows, "Front Row");
        for (int i = 0; i < backRow.transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(backRow, "Unit Space " + i);
            if (unitSpace) backRowSpaces[i] = unitSpace;
        }
        for (int i = 0; i < frontRow.transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(frontRow, "Unit Space " + i);
            if (unitSpace) frontRowSpaces[i] = unitSpace;
        }
    }

    void FillUnitSpace(GameObject unitSpace, MapUnit unit) {
        GameObject NameText = Tools.GetChildNamed(unitSpace, "Name Text");
        GameObject HealthText = Tools.GetChildNamed(unitSpace, "Health Text");
        GameObject Portrait = Tools.GetChildNamed(unitSpace, "Portrait");
        NameText.GetComponent<TextMesh>().text = unit.name;
        HealthText.GetComponent<TextMesh>().text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
        Portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Portraits/" + unit.portraitName);
    }

    void CleanUnitSpaces() {
        for (int i = 0; i < backRowSpaces.Length; i++) {
            if (backRowSpaces[i] != null) {
                GameObject HealthText = Tools.GetChildNamed(backRowSpaces[i], "Name Text");
                GameObject AbilityText = Tools.GetChildNamed(backRowSpaces[i], "Health Text");
                GameObject Portrait = Tools.GetChildNamed(backRowSpaces[i], "Portrait");

                HealthText.GetComponent<TextMesh>().text = "";
                AbilityText.GetComponent<TextMesh>().text = "";
                Portrait.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        for (int i = 0; i < frontRowSpaces.Length; i++) {
            if (frontRowSpaces[i] != null) {
                GameObject HealthText = Tools.GetChildNamed(frontRowSpaces[i], "Name Text");
                GameObject AbilityText = Tools.GetChildNamed(frontRowSpaces[i], "Health Text");
                GameObject Portrait = Tools.GetChildNamed(frontRowSpaces[i], "Portrait");

                HealthText.GetComponent<TextMesh>().text = "";
                AbilityText.GetComponent<TextMesh>().text = "";
                Portrait.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
}
