using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShopSpace : MonoBehaviour
{
    GameObject portrait;
    GameObject moneyCost;
    GameObject zealCost;
    GameObject unitName;
    GameObject unitPower;
    GameObject army;
    GameObject nodeMenu;
    GameObject unitShop;
    GameObject human;
    //Faction currentFaction;
    int unitNumber;

    public MapUnit unit;



    // Start is called before the first frame update
    void Start(){
        InitializeMembers();
    }

    // Update is called once per frame
    void Update(){
    }

    public void InitializeMembers() {
        portrait = Tools.GetChildNamed(gameObject, "Portrait");
        moneyCost = Tools.GetChildNamed(gameObject, "Money Cost Text");
        zealCost = Tools.GetChildNamed(gameObject, "Zeal Cost Text");
        unitName = Tools.GetChildNamed(gameObject, "Name Text");
        unitPower = Tools.GetChildNamed(gameObject, "Power Text");
        unitNumber = int.Parse(name.Substring(name.Length - 1));
        nodeMenu = GameObject.Find("/Node Menu");
        unitShop = GameObject.Find("/Unit Buying Menu");
        human = Player.human;
    }



    private void OnMouseDown() {
        if (human.GetComponent<Player>().money >= unit.moneyCost && human.GetComponent<Player>().zeal >= unit.zealCost) {
            BuyUnit(NodeMenu.currentArmy, UnitSpace.currentUnitPos);
        }
    }

    public void AddUnit(MapUnit newUnit) {
        unit = newUnit;
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Factions/" + unit.faction + "/Portrait/" + unit.name);
        moneyCost.GetComponent<TextMesh>().text = unit.moneyCost.ToString();
        zealCost.GetComponent<TextMesh>().text = unit.zealCost.ToString();
        unitName.GetComponent<TextMesh>().text = unit.name;
        unitPower.GetComponent<TextMesh>().text = unit.power.ToString();
    }
    public void BuyUnit(GameObject army, UnitPos unitPos) {
        //print("buying unit");
        army.GetComponent<Army>().BuyUnit(unitPos, unit);
        LeaveMenu();
    }
    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }
}
