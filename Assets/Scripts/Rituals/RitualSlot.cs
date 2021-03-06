﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualSlot : MonoBehaviour{

    public Ritual ritualBlueprint;

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 2) {
            Ritual ritualToBuy = ritualBlueprint.DeepCopy();
            if (NodeMenu.currentNode.GetComponent<Node>().temple != null && NodeMenu.currentNode.GetComponent<Node>().temple.name == TempleName.Tradition) {
                ritualToBuy.zealCost--;
            }
            if (Player.human.GetComponent<Player>().BuyRitual(ritualToBuy)) {
                transform.parent.GetComponent<RitualMenu>().ExitMenu();
            }
            else Tools.CreatePopup(gameObject, "Not Enough Zeal", 40, Color.red);
        }
    }
}
