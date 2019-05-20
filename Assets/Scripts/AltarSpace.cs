﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSpace : MonoBehaviour{

    GameObject altarBuyingMenu;
    public static GameObject currentAltarSpace;

    // Start is called before the first frame update
    void Start(){
        altarBuyingMenu = GameObject.Find("/Altar Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        print("Altar space clicked");
        if (Player.menuOpen==false) {
            print("altar space happening");
            altarBuyingMenu.GetComponent<AltarShopManager>().EnterMenu();
            currentAltarSpace = gameObject;
            Player.menuOpen = true;
        }
    }
}

