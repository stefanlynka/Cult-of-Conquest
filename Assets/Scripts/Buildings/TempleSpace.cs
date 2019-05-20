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
        if (Player.menuOpen == false) {
            templeBuyingMenu.GetComponent<TempleShopManager>().EnterMenu();
            currentTempleSpace = gameObject;
            Player.menuOpen = true;
        }
    }
}
