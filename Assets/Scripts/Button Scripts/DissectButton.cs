﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissectButton : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update() {
    }

    private void OnMouseDown() {
        if (ShouldDissect(NodeMenu.currentArmy, UnitSpace.currentUnitPos)) DissectUnit(NodeMenu.currentArmy, NodeMenu.currentArmy.GetComponent<Army>().GetUnit(UnitSpace.currentUnitPos));
    }

    bool ShouldDissect(GameObject army, UnitPos position) {
        MapUnit unit = army.GetComponent<Army>().GetUnit(position);
        if (unit != null) {
            print("unit not null");
            if (unit.race != Player.human.GetComponent<Player>().race) {
                print("unit of a different race");
                return true;
            }
        }
        return false;
    }

    void DissectUnit(GameObject army, MapUnit unit) {
        print("dissect!");
        army.GetComponent<Army>().RemoveUnit(unit);
        army.GetComponent<Army>().owner.GetComponent<Player>().zeal++;
        transform.parent.GetComponent<UnitShopManager>().LeaveMenu();
    }

}
