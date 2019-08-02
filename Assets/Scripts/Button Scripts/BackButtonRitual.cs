using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonRitual : MonoBehaviour{

    GameObject ritualMenu;
    // Start is called before the first frame update

    void Start() {
        ritualMenu = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        ritualMenu.GetComponent<RitualMenu>().ExitMenu();
        //ritualMenu.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        //UnitSpace.buyingMenuOpen = false;
        Player.menuOpen = 1;
    }
}
