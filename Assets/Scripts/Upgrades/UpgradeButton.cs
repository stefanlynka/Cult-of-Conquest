﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour{

    public Upgrade upgrade;
    GameObject indicators, dissectedMenu;

    // Start is called before the first frame update
    void Start(){
        indicators = Tools.GetChildNamed(gameObject, "Upgrade Indicators");
        dissectedMenu = GameObject.Find("/Dissected Faction Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            if (Player.human.GetComponent<Player>().upgradesBackup.Count != 0 ) {
                if (!Player.human.GetComponent<Player>().upgradesBackup.ContainsKey(upgrade.name)) {
                    if (Player.human.GetComponent<Player>().upgradesBackup.ContainsKey("Adapt 1")) {
                        Player.human.GetComponent<Player>().upgradesBackup.Remove("Adapt 1");
                        Player.human.GetComponent<Player>().upgradesBackup.Add(upgrade.name, upgrade);
                        print("New Upgrade Acquired 1");
                    }
                    else if (Player.human.GetComponent<Player>().upgradesBackup.ContainsKey("Adapt 2")) {
                        Player.human.GetComponent<Player>().upgradesBackup.Remove("Adapt 2");
                        Player.human.GetComponent<Player>().upgradesBackup.Add(upgrade.name, upgrade);
                        print("New Upgrade Acquired 1");
                    }
                    Player.human.GetComponent<Player>().upgrades.Clear();
                    Player.human.GetComponent<Player>().upgrades = new Dictionary<string, Upgrade>(Player.human.GetComponent<Player>().upgradesBackup);
                    Player.human.GetComponent<Player>().upgradesBackup.Clear();
                    transform.parent.transform.parent.GetComponent<UpgradeManager>().LoadHumanUpgrades(Player.human);
                    //transform.parent.transform.parent.GetComponent<UpgradeMenu>().ExitMenu();
                    print("New Upgrade Acquired 2");
                }
            }
            else if (upgrade.name.Contains("Adapt")){
                dissectedMenu.GetComponent<DissectedFactionMenu>().EnterMenu("Adapt");
            }
            else if (Player.human.GetComponent<Player>().BuyUpgrade(upgrade)) {
                UpdateIndicator(upgrade);
                //transform.parent.transform.parent.GetComponent<UpgradeMenu>().ExitMenu();
            }
        }
    }
    void UpdateIndicator(Upgrade upgrade) {
        for (int i = 0; i < upgrade.currentLevel; i++) {
            GameObject indicator = Tools.GetChildNamed(indicators, "Unupgraded Indicator " + i.ToString());
            indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Upgraded Indicator");
        }
    }
}
