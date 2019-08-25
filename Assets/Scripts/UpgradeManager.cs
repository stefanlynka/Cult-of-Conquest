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
            }
        }
    }

    void NoumenonUpgrades(GameObject player) {
        Upgrade upgrade1 = new Upgrade("Strike Hard", 5, 3, "Increase damage \nwhen attacking");
        player.GetComponent<Player>().upgrades.Add(upgrade1.name, upgrade1);
        LoadPlayerUpgrade(0, upgrade1, player);
        Upgrade upgrade2 = new Upgrade("Cover Your Tracks", 5, 3, "Increase \nfog of war");
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
