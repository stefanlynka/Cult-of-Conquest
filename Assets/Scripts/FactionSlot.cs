using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSlot : MonoBehaviour{

    Faction faction;
    

    // Start is called before the first frame update
    void Start(){
        faction = Tools.StringToFaction(name);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        if (transform.parent.GetComponent<DissectedFactionMenu>().situation == "Temple") {
            GameObject.Find("/Temple Buying Menu").GetComponent<TempleShopManager>().MakeTemples(faction);
            GameObject.Find("/Temple Buying Menu").GetComponent<TempleShopManager>().EnterMenu();
        }
        else if (transform.parent.GetComponent<DissectedFactionMenu>().situation == "Altar") {
            GameObject.Find("/Altar Buying Menu").GetComponent<AltarShopManager>().MakeAltars(faction);
            GameObject.Find("/Altar Buying Menu").GetComponent<AltarShopManager>().EnterMenu();
        }
        else if (transform.parent.GetComponent<DissectedFactionMenu>().situation == "Adapt") {
            Player.human.GetComponent<Player>().upgradesBackup = new Dictionary<string, Upgrade>(Player.human.GetComponent<Player>().upgrades);
            Player.human.GetComponent<Player>().upgrades.Clear();
            GameObject.Find("/Upgrade Menu").GetComponent<UpgradeManager>().SetPlayerUpgrades(Player.human, faction);
            transform.parent.GetComponent<DissectedFactionMenu>().ExitMenu();
        }
    }
}
