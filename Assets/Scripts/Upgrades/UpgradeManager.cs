using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour{

    List<GameObject> players = new List<GameObject>();
    GameObject human;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void Startup() {
        InitializeMembers();
    }

    void InitializeMembers() {
        GameObject playerList = GameObject.Find("/Players");
        for (int i = 0; i < playerList.transform.childCount; i++) {
            GameObject child = playerList.transform.GetChild(i).gameObject;
            if (child.GetComponent<Player>()) players.Add(child);
        }
        human = Player.human;
        SetupUpgrades();
    }



    void SetupUpgrades() {
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            switch (player.GetComponent<Player>().faction) {
                case Faction.Noumenon:
                    NoumenonUpgrades(player);
                    break;
                case Faction.Dukkha:
                    ZenteelUpgrades(player);
                    break;
                case Faction.Paratrophs:
                    ParatrophsUpgrades(player);
                    break;
                case Faction.Unmar:
                    UnmarUpgrades(player);
                    break;
                case Faction.Samata:
                    SamataUpgrades(player);
                    break;
                case Faction.Carnot:
                    CarnotUpgrades(player);
                    break;
            }
        }
    }

    public void LoadPlayerUpgrade(int index, Upgrade upgrade, GameObject player) {
        if (player == Player.human) {
            GameObject upgradeSlots = Tools.GetChildNamed(gameObject, "Upgrade Slots");
            GameObject upgradeSlot = Tools.GetChildNamed(upgradeSlots, "Upgrade Slot " + index.ToString());
            if (upgradeSlot != null) {
                upgradeSlot.GetComponent<UpgradeButton>().upgrade = upgrade;
                Tools.GetChildNamed(upgradeSlot, "Upgrade Name Text").GetComponent<TextMesh>().text = upgrade.name;
                Tools.GetChildNamed(upgradeSlot, "Upgrade Cost Text").GetComponent<TextMesh>().text = upgrade.zealCost.ToString();
                Tools.GetChildNamed(upgradeSlot, "Upgrade Description Text").GetComponent<TextMesh>().text = upgrade.description;
                if (upgrade.maxLevel == 2) {
                    GameObject indicators = Tools.GetChildNamed(upgradeSlot, "Upgrade Indicators");
                    GameObject indicator3 = Tools.GetChildNamed(indicators, "Unupgraded Indicator 2");
                    indicator3.SetActive(false);
                }
                if (upgrade.maxLevel == 1) {
                    GameObject indicators = Tools.GetChildNamed(upgradeSlot, "Upgrade Indicators");
                    GameObject indicator2 = Tools.GetChildNamed(indicators, "Unupgraded Indicator 1");
                    indicator2.SetActive(false);
                }
            }
        }
    }

    void NoumenonUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("Strike First", 5, 3, "Increase damage \nwhen attacking");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("Cover Your Tracks", 8, 2, "Increase \nfog of war");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("u3", 5, 3, "stronger");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }
    void ZenteelUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("u1", 5, 3, "harder");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("u2", 5, 3, "faster");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("u3", 5, 3, "stronger");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }
    void ParatrophsUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("Assimilate", 5, 3, "Rebuild destroyed buildings.\nBuild enemy units.\nBuild enemy buildings");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("Adapt 1", 5, 3, "Unlock an\nupgrade from\nanother faction");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("Adapt 2", 5, 3, "Unlock an\nupgrade from\nanother faction");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }
    void UnmarUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("u1", 5, 3, "harder");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("u2", 5, 3, "faster");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("u3", 5, 3, "stronger");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }
    void SamataUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("Last One Standing", 5, 3, "Strengthen your\nlast unit\nin a battle");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("Against Tyranny", 5, 3, "Increase damage\nwhen attacking strongest\nenemy faction");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("Reap Just Rewards", 5, 3, "Increase rewards\nfrom winning\na fair fight");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }
    void CarnotUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("u1", 5, 3, "harder");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("u2", 5, 3, "faster");
        player.GetComponent<Player>().upgrades.Add(upgrade2.name, upgrade2);
        LoadPlayerUpgrade(1, upgrade2, player);
        Upgrade upgrade3 = new Upgrade("u3", 5, 3, "stronger");
        player.GetComponent<Player>().upgrades.Add(upgrade3.name, upgrade3);
        LoadPlayerUpgrade(2, upgrade3, player);
    }



}
