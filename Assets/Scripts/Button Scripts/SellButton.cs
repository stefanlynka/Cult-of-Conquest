using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour { 
    GameObject unitShop;
    // Start is called before the first frame update
    void Start(){
        unitShop = GameObject.Find("/Unit Buying Menu");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        NodeMenu.currentArmy.GetComponent<Army>().SellUnit(NodeMenu.currentArmy.GetComponent<Army>().GetUnit(UnitSpace.currentUnitPos));
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
        UnitSpace.buyingMenuOpen = false;
        Player.menuOpen = 1;
        Tools.GetChildNamed(transform.parent.transform.parent.gameObject,"Unit Spaces").GetComponent<UnitShopManager>().LeaveMenu();
    }
}