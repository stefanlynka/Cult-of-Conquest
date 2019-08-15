using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpace : MonoBehaviour
{
    GameObject unitBuyingSpaces;
    public static bool buyingMenuOpen = false;
    public static GameObject currentUnitSpace;
    public static UnitPos currentUnitPos;

    public int position;
    public bool frontRow;
    public bool shop = false;


    // Start is called before the first frame update
    void Start(){
        unitBuyingSpaces = GameObject.Find("/Unit Buying Menu/Unit Spaces");
    }

    // Update is called once per frame
    void Update(){
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            //print("occupied = " + NodeMenu.currentNode.GetComponent<Node>().occupied);
            if (NodeMenu.currentNode.GetComponent<Node>().occupied) {
                currentUnitSpace = gameObject;
                currentUnitPos = new UnitPos(position, frontRow);
                unitBuyingSpaces.GetComponent<UnitShopManager>().EnterMenu();
                Player.menuOpen = 2;
            }
        }
    }

    public void LoadUnit(MapUnit unit) {
        GameObject NameText = Tools.GetChildNamed(gameObject, "Name Text");
        GameObject HealthText = Tools.GetChildNamed(gameObject, "Health Text");
        GameObject Portrait = Tools.GetChildNamed(gameObject, "Portrait");
        GameObject Shield = Tools.GetChildNamed(gameObject, "Shield");

        if (unit == null) {
            NameText.GetComponent<TextMesh>().text = "";
            HealthText.GetComponent<TextMesh>().text = "";
            Portrait.GetComponent<SpriteRenderer>().sprite = null;
            Shield.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            NameText.GetComponent<TextMesh>().text = unit.name;
            HealthText.GetComponent<TextMesh>().text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
            Portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Races/" + unit.race + "/Portrait/" + unit.portraitName);
            if (unit.currentShield == 0) Shield.GetComponent<SpriteRenderer>().sprite = null;
            if (unit.currentShield == 1) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield1");
            if (unit.currentShield == 2) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield2");
            if (unit.currentShield == 3) Shield.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Shield3");
        }
    }
}

