using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour{


    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        MapUnit unit = NodeMenu.currentArmy.GetComponent<Army>().GetUnit(UnitSpace.currentUnitPos);
        if (Player.human.GetComponent<Player>().money >= unit.moneyCost / 4) HideUnit(unit);
    }

    void HideUnit(MapUnit unit) {
        unit.power = 0;
        unit.hidden = true;
        transform.parent.GetComponent<UnitShopManager>().LeaveMenu();
    }
}
