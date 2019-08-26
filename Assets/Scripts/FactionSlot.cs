using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSlot : MonoBehaviour{

    Faction faction;
    

    // Start is called before the first frame update
    void Start(){
        faction = Tools.StringToFaction(name);
        print(faction);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {

        if (transform.parent.GetComponent<DissectedFactionMenu>().buildingType == "Temple") {
            GameObject.Find("/Temple Buying Menu").GetComponent<TempleShopManager>().MakeTemples(faction);
            GameObject.Find("/Temple Buying Menu").GetComponent<TempleShopManager>().EnterMenu();
        }
        else {
            GameObject.Find("/Altar Buying Menu").GetComponent<AltarShopManager>().MakeAltars(faction);
            GameObject.Find("/Altar Buying Menu").GetComponent<AltarShopManager>().EnterMenu();
        }
    }
}
