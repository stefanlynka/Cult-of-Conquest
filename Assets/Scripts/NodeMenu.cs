using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMenu : MonoBehaviour
{
    public static GameObject currentNode;
    public static GameObject currentArmy;
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
        currentNode = node;
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -10));
        //print("node = " + node);
        //print("occupant = "+node.GetComponent<Node>().GetOccupant());
        currentArmy = node.GetComponent<Node>().GetOccupant();
        if (currentArmy) LoadArmy();
        LoadAltar();
        LoadTemple();
        open = true;
        Player.menuOpen = 1;
    }

    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 11, -10));
        CleanUnitSpaces();
        open = false;
        Player.menuOpen = 0;
    }

    public void LoadArmy() {
        print("Loading Army");
        //print("current army:" + currentArmy);
        MapUnit[] backUnits = currentArmy.GetComponent<Army>().backRow;
        MapUnit[] frontUnits = currentArmy.GetComponent<Army>().frontRow;
        for (int i = 0; i < backUnits.Length; i++) {
            if (backUnits[i] != null) {
                backRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(backUnits[i]);
            }
        }
        for (int i = 0; i < frontUnits.Length; i++) {
            if (frontUnits[i] != null) {
                frontRowSpaces[i].GetComponent<UnitSpace>().LoadUnit(frontUnits[i]);
            }
        }
    }

    public void LoadAltar() {
        if (currentNode.GetComponent<Node>().altar != null) {
            Altar altar = currentNode.GetComponent<Node>().altar;
            GameObject locationMenu = Tools.GetChildNamed(gameObject, "Location Menu");
            GameObject altarSpace = Tools.GetChildNamed(locationMenu, "Altar Space");
            GameObject titleText = Tools.GetChildNamed(altarSpace, "Altar Title Text");
            GameObject altarSprite = Tools.GetChildNamed(altarSpace, "Altar Sprite");
            GameObject descriptionText = Tools.GetChildNamed(altarSpace, "Altar Description Text");
            
            titleText.GetComponent<TextMesh>().text = altar.name.ToString();
            altarSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Altars/Altar of " + altar.portrait);
            descriptionText.GetComponent<TextMesh>().text = altar.description;
        }
    }

    public void LoadTemple() {
        //print("Temple Loading");
        if (currentNode.GetComponent<Node>().temple != null) {
            Temple temple = currentNode.GetComponent<Node>().temple;
            GameObject locationMenu = Tools.GetChildNamed(gameObject, "Location Menu");
            GameObject templeSpace = Tools.GetChildNamed(locationMenu, "Temple Space");
            GameObject titleText = Tools.GetChildNamed(templeSpace, "Temple Title Text");
            GameObject templeSprite = Tools.GetChildNamed(templeSpace, "Temple Sprite");
            GameObject descriptionText = Tools.GetChildNamed(templeSpace, "Temple Description Text");

            titleText.GetComponent<TextMesh>().text = temple.name.ToString();
            templeSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Temples/Temple of " + temple.portrait);
            descriptionText.GetComponent<TextMesh>().text = temple.description;
        }
    }

    void FindUnitSpaces() {
        GameObject armyMenu = Tools.GetChildNamed(gameObject, "Army Menu");
        GameObject unitRows = Tools.GetChildNamed(armyMenu, "Unit Rows");
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

    /*
    void FillUnitSpace(GameObject unitSpace, MapUnit unit) {
        GameObject NameText = Tools.GetChildNamed(unitSpace, "Name Text");
        GameObject HealthText = Tools.GetChildNamed(unitSpace, "Health Text");
        GameObject Portrait = Tools.GetChildNamed(unitSpace, "Portrait");
        GameObject Shield = Tools.GetChildNamed(unitSpace, "Shield");
        NameText.GetComponent<TextMesh>().text = unit.name;
        HealthText.GetComponent<TextMesh>().text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
        Portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Races/" + unit.race + "/Portrait/" + unit.portraitName);
        if (unit.currentShield == 0) Shield.GetComponent<SpriteRenderer>().sprite = null;
        if (unit.currentShield == 1) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield1");
        if (unit.currentShield == 2) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield2");
        if (unit.currentShield == 3) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield3");
    }
    */

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
        GameObject locationMenu = Tools.GetChildNamed(gameObject, "Location Menu");
        GameObject altarSpace = Tools.GetChildNamed(locationMenu, "Altar Space");
        GameObject templeSpace = Tools.GetChildNamed(locationMenu, "Temple Space");

        GameObject titleText = Tools.GetChildNamed(altarSpace, "Altar Title Text");
        GameObject altarSprite = Tools.GetChildNamed(altarSpace, "Altar Sprite");
        GameObject descriptionText = Tools.GetChildNamed(altarSpace, "Altar Description Text");
        titleText.GetComponent<TextMesh>().text = "Altar";
        altarSprite.GetComponent<SpriteRenderer>().sprite = null;
        descriptionText.GetComponent<TextMesh>().text = "Click to\nbuild Altar";

        titleText = Tools.GetChildNamed(templeSpace, "Temple Title Text");
        GameObject templeSprite = Tools.GetChildNamed(templeSpace, "Temple Sprite");
        descriptionText = Tools.GetChildNamed(templeSpace, "Temple Description Text");
        titleText.GetComponent<TextMesh>().text = "Temple";
        templeSprite.GetComponent<SpriteRenderer>().sprite = null;
        descriptionText.GetComponent<TextMesh>().text = "Click to\nbuild Temple";

    }
}
