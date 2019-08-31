using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyMenu : MonoBehaviour {

    GameObject[] backRowSpaces = new GameObject[5];
    GameObject[] frontRowSpaces = new GameObject[5];
    public GameObject army;

    // Start is called before the first frame update
    void Start() {
        FindUnitSpaces();
    }

    // Update is called once per frame
    void Update() {

    }

    void FindUnitSpaces() {
        GameObject unitRows = Tools.GetChildNamed(gameObject, "Unit Rows");
        GameObject backRow = Tools.GetChildNamed(unitRows, "Back Row");
        GameObject frontRow = Tools.GetChildNamed(unitRows, "Front Row");
        for (int i = 0; i < backRow.transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(backRow, "Unit Space " + i);
            if (unitSpace) {
                unitSpace.GetComponent<UnitSpace>().position = i;
                unitSpace.GetComponent<UnitSpace>().frontRow = false;
                backRowSpaces[i] = unitSpace;
            }
        }
        for (int i = 0; i < frontRow.transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(frontRow, "Unit Space " + i);
            if (unitSpace) {
                unitSpace.GetComponent<UnitSpace>().position = i;
                unitSpace.GetComponent<UnitSpace>().frontRow = true;
                frontRowSpaces[i] = unitSpace;
            }
        }
    }

    public void LoadArmy(GameObject newArmy) {
        print("Loading Army");
        MapUnit[] backUnits = newArmy.GetComponent<Army>().backRow;
        MapUnit[] frontUnits = newArmy.GetComponent<Army>().frontRow;
        for (int i = 0; i < backUnits.Length; i++) {
            if (backUnits[i] != null) {
                backRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(backUnits[i]);
            }
            else if (backRowSpaces[i] != null) backRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(null);
        }
        for (int i = 0; i < frontUnits.Length; i++) {
            if (frontUnits[i] != null) {
                frontRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(frontUnits[i]);
            }
            else if (frontRowSpaces[i] != null) frontRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(null);

        }
    }

    /*
    void FillUnitSpace(GameObject unitSpace, MapUnit unit) {
        GameObject NameText = Tools.GetChildNamed(unitSpace, "Name Text");
        GameObject HealthText = Tools.GetChildNamed(unitSpace, "Health Text");
        GameObject Portrait = Tools.GetChildNamed(unitSpace, "Portrait");
        GameObject Shield = Tools.GetChildNamed(unitSpace, "Shield");

        if (unit == null) {
            NameText.GetComponent<TextMesh>().text = "";
            HealthText.GetComponent<TextMesh>().text = "";
            Portrait.GetComponent<SpriteRenderer>().sprite = null;
            Shield.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            NameText.GetComponent<TextMesh>().text = unit.name;
            HealthText.GetComponent<TextMesh>().text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
            Portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Factions/" + unit.faction + "/Portrait/" + unit.portraitName);
            if (unit.currentShield == 0) Shield.GetComponent<SpriteRenderer>().sprite = null;
            if (unit.currentShield == 1) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield1");
            if (unit.currentShield == 2) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield2");
            if (unit.currentShield == 3) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield3");
        }
    }
    */
    void CleanUnitSpaces() {
        for (int i = 0; i < backRowSpaces.Length; i++) {
            if (backRowSpaces[i] != null) {
                GameObject nameText = Tools.GetChildNamed(backRowSpaces[i], "Name Text");
                GameObject healthText = Tools.GetChildNamed(backRowSpaces[i], "Health Text");
                GameObject portrait = Tools.GetChildNamed(backRowSpaces[i], "Portrait");

                nameText.GetComponent<TextMesh>().text = "";
                healthText.GetComponent<TextMesh>().text = "";
                portrait.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    public void LoadBuildings(GameObject defendingNode) {
        GameObject protection = Tools.GetChildNamed(gameObject, "Protection Buildings");
        GameObject templeSpace = Tools.GetChildNamed(protection, "Temple Space");
        GameObject altarSpace = Tools.GetChildNamed(protection, "Altar Space");

        GameObject nameText = Tools.GetChildNamed(templeSpace, "Name Text");
        GameObject healthText = Tools.GetChildNamed(templeSpace, "Health Text");
        GameObject portrait = Tools.GetChildNamed(templeSpace, "Portrait");
        if (defendingNode.GetComponent<Node>().temple != null && defendingNode.GetComponent<Node>().temple.name == TempleName.Protection) {
            nameText.GetComponent<TextMesh>().text = "Temple of \nProtection";
            healthText.GetComponent<TextMesh>().text = "HP: " + defendingNode.GetComponent<Node>().temple.unit.currentHealth;
            portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Temples/Temple of Protection");
        }
        else {
            nameText.GetComponent<TextMesh>().text = "Temple";
            healthText.GetComponent<TextMesh>().text = "";
            portrait.GetComponent<SpriteRenderer>().sprite = null;
        }

        nameText = Tools.GetChildNamed(altarSpace, "Name Text");
        healthText = Tools.GetChildNamed(altarSpace, "Health Text");
        portrait = Tools.GetChildNamed(altarSpace, "Portrait");
        if (defendingNode.GetComponent<Node>().altar != null && defendingNode.GetComponent<Node>().altar.name == AltarName.Conflict) {
            nameText.GetComponent<TextMesh>().text = "Altar of \nConflict";
            healthText.GetComponent<TextMesh>().text = "HP: " + defendingNode.GetComponent<Node>().altar.unit.currentHealth;
            portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Altars/Altar of Conflict");
        }
        else {
            nameText.GetComponent<TextMesh>().text = "Altar";
            healthText.GetComponent<TextMesh>().text = "";
            portrait.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
