using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonAltar : MonoBehaviour{

    GameObject altarShop;
    // Start is called before the first frame update
    void Start() {
        altarShop = GameObject.Find("/Altar Buying Menu");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        altarShop.GetComponent<AltarShopManager>().ExitMenu();
    }
}
