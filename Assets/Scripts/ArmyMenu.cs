﻿using System.Collections;
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
                FillUnitSpace(backRowSpaces[i], backUnits[i]);
            }
            else if (backRowSpaces[i] != null) FillUnitSpace(backRowSpaces[i], null);
        }
        for (int i = 0; i < frontUnits.Length; i++) {
            if (frontUnits[i] != null) {
                FillUnitSpace(frontRowSpaces[i], frontUnits[i]);
            }
            else if (frontRowSpaces[i] != null) FillUnitSpace(frontRowSpaces[i], null);

        }
    }

    void FillUnitSpace(GameObject unitSpace, MapUnit unit) {
        GameObject NameText = Tools.GetChildNamed(unitSpace, "Name Text");
        GameObject HealthText = Tools.GetChildNamed(unitSpace, "Health Text");
        GameObject Portrait = Tools.GetChildNamed(unitSpace, "Portrait");

        if (unit == null) {
            NameText.GetComponent<TextMesh>().text = "";
            HealthText.GetComponent<TextMesh>().text = "";
            Portrait.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            NameText.GetComponent<TextMesh>().text = unit.name;
            HealthText.GetComponent<TextMesh>().text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
            Portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Races/" + unit.race + "/Portrait/" + unit.portraitName);
        }
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
    }
}