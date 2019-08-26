using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleSpace : MonoBehaviour{

    GameObject templeBuyingMenu;
    public static GameObject currentTempleSpace;

    // Start is called before the first frame update
    void Start() {
        templeBuyingMenu = GameObject.Find("/Temple Buying Menu");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            currentTempleSpace = gameObject;
            if (Player.human.GetComponent<Player>().upgrades.ContainsKey("Assimilate") && Player.human.GetComponent<Player>().upgrades["Assimilate"].currentLevel >= 3) {
                GameObject.Find("/Dissected Faction Menu").GetComponent<DissectedFactionMenu>().EnterMenu("Temple");
            }
            else {
                templeBuyingMenu.GetComponent<TempleShopManager>().EnterMenu();
                Player.menuOpen = 2;
            }
        }
    }
}
