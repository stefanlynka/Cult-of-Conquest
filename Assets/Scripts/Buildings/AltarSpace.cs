using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSpace : MonoBehaviour{

    GameObject altarBuyingMenu;
    public static GameObject currentAltarSpace;

    // Start is called before the first frame update
    void Start() {
        altarBuyingMenu = GameObject.Find("/Altar Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            currentAltarSpace = gameObject;
            if (Player.human.GetComponent<Player>().upgrades.ContainsKey("Assimilate") && Player.human.GetComponent<Player>().upgrades["Assimilate"].currentLevel >= 3) {
                GameObject.Find("/Dissected Faction Menu").GetComponent<DissectedFactionMenu>().EnterMenu("Altar");
            }
            else {
                altarBuyingMenu.GetComponent<AltarShopManager>().EnterMenu();
                Player.menuOpen = 2;
            }
        }
    }
}

