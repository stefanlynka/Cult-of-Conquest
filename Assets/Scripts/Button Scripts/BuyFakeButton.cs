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
        unit.maxDamage = 0;
        unit.moneyCost = int.Parse(transform.GetChild(0).GetComponent<TextMesh>().text);
        unit.zealCost = 0;
        unit.name += "Fake";
        unit.portraitName += "Fake";
    }

    private void OnMouseDown() {
        if (Player.human.GetComponent<Player>().money >= unit.moneyCost && Player.human.GetComponent<Player>().zeal >= unit.zealCost) {
            BuyUnit(NodeMenu.currentArmy, UnitSpace.currentUnitPos);
        }
    }

    public void BuyUnit(GameObject army, UnitPos unitPos) {
        army.GetComponent<Army>().BuyUnit(unitPos, unit);
        LeaveMenu();
    }
    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }
}
