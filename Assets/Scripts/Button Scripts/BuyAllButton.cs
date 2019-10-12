using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAllButton : MonoBehaviour{

    MapUnit unit;
    GameObject nodeMenu, unitShop;
    // Start is called before the first frame update
    void Start(){
        nodeMenu = GameObject.Find("/Node Menu");
        unitShop = GameObject.Find("/Unit Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SetUnit(MapUnit newUnit) {
        unit = newUnit.DeepCopy();
    }

    private void OnMouseDown() {
        int unitCost = NodeMenu.currentNode.GetComponent<Node>().GetUnitCost(Tools.UnitToIndex(unit));
        if (Player.human.GetComponent<Player>().money >= unitCost && Player.human.GetComponent<Player>().zeal >= unit.zealCost) {
            BuyUnit(NodeMenu.currentArmy);
        }
        else Tools.CreatePopup(gameObject, "Not Enough Money", 40, Color.yellow);
    }

    public void BuyUnit(GameObject army) {
        //print("buying unit");
        MapUnit unitToBuy = unit.DeepCopy();
        if (NodeMenu.currentNode.GetComponent<Node>().temple != null && NodeMenu.currentNode.GetComponent<Node>().temple.name == TempleName.Armaments) {
            unitToBuy.moneyCost = (int)(unitToBuy.moneyCost * 0.6f);
        }
        for(int i=0; i< 4; i++) {
            UnitPos unitPos = new UnitPos(i, true);
            if (army.GetComponent<Army>().IsSpotOpen(unitPos)) army.GetComponent<Army>().BuyUnit(unitPos, unitToBuy);
        }
        for (int i = 0; i < 4; i++) {
            UnitPos unitPos = new UnitPos(i, false);
            if (army.GetComponent<Army>().IsSpotOpen(unitPos)) army.GetComponent<Army>().BuyUnit(unitPos, unitToBuy);
        }
        LeaveMenu();
    }
    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }
}
