using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFakeButton : MonoBehaviour{

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
    public void InitializeMembers() {
        unit = transform.parent.GetComponent<UnitShopSpace>().unit.DeepCopy();
        //unit.moneyCost = Mathf.RoundToInt(unit.moneyCost * 0.4f);
        //unit.zealCost = 0;
        //unit.name += "Fake";
        //unit.portraitName += "Fake";
        unit.fake = true;
    }

    private void OnMouseDown() {
        if (Player.human.GetComponent<Player>().money >= Mathf.RoundToInt(unit.moneyCost * 0.4f) && Player.human.GetComponent<Player>().zeal >= unit.zealCost) {
            BuyUnit(NodeMenu.currentArmy, UnitSpace.currentUnitPos);
        }
    }

    public void BuyUnit(GameObject army, UnitPos unitPos) {
        army.GetComponent<Army>().BuyFakeUnit(unitPos, unit.DeepCopy());
        LeaveMenu();
    }
    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }
}
