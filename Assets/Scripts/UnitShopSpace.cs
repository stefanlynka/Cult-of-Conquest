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
    }



    private void OnMouseDown() {
        int unitCost = NodeMenu.currentNode.GetComponent<Node>().GetUnitCost(Tools.UnitToIndex(unit));
        if (Player.human.GetComponent<Player>().money >= unitCost && Player.human.GetComponent<Player>().zeal >= unit.zealCost) {
            BuyUnit(NodeMenu.currentArmy, UnitSpace.currentUnitPos);
        }
        else Tools.CreatePopup(gameObject, "Not Enough Money", 40, Color.yellow);
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
        MapUnit unitToBuy = unit.DeepCopy();
        if (NodeMenu.currentNode.GetComponent<Node>().temple != null && NodeMenu.currentNode.GetComponent<Node>().temple.name == TempleName.Armaments) {
            unitToBuy.moneyCost = (int)(unitToBuy.moneyCost * 0.6f);
        }
        army.GetComponent<Army>().BuyUnit(unitPos, unitToBuy);
        LeaveMenu();
    }
    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }

    public void UpdateUnit() {
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Factions/" + unit.faction + "/Portrait/" + unit.name);
        int unitCost = NodeMenu.currentNode.GetComponent<Node>().GetUnitCost(Tools.UnitToIndex(unit));
        moneyCost.GetComponent<TextMesh>().text = unitCost.ToString();
        zealCost.GetComponent<TextMesh>().text = unit.zealCost.ToString();
        unitName.GetComponent<TextMesh>().text = unit.name;
        unitPower.GetComponent<TextMesh>().text = unit.power.ToString();
    }
}
