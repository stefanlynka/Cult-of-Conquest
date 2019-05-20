using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonTemple : MonoBehaviour{

    GameObject templeShop;
    // Start is called before the first frame update
    void Start() {
        templeShop = GameObject.Find("/Temple Buying Menu");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        templeShop.GetComponent<TempleShopManager>().ExitMenu();
    }
}
