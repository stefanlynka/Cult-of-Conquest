using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShopManager : MonoBehaviour {

    GameObject army, nodeMenu, unitShop, dissectButton;
    //Faction currentFaction;
    int unitSpaceCount = 4;
    GameObject[] unitSpaces;


    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void Startup() {
        InitializeMembers();
    }


    public void InitializeMembers() {
        unitSpaces = new GameObject[unitSpaceCount];
        for (int i = 0; i < transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(gameObject, "Buy Unit Space " + i);
            if (unitSpace) unitSpaces[i] = unitSpace;
        }

        //currentFaction = TurnManager.human.GetComponent<Player>().faction;

        transform.parent.gameObject.GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));

        nodeMenu = GameObject.Find("/Node Menu");
        unitShop = GameObject.Find("/Unit Buying Menu");
        dissectButton = Tools.GetChildNamed(gameObject, "Dissect Button");

        //army = NodeMenu.currentArmy;
        //if (army) currentFaction = army.GetComponent<Army>().faction;
        List<GameObject> players = Controller.players;
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            MakeUnits(player.GetComponent<Player>().faction, player);
            if (!player.GetComponent<AI>()) {
                AssignUnits(player);
                //SetupUnitShopSpaces();
            }
        }
    }

    public void SetupUnitShopSpaces() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name.Contains("Buy Unit Space")) {
                if (Player.human.GetComponent<Player>().faction == Faction.Noumenon) Tools.GetChildNamed(child, "Fake").GetComponent<BuyFakeButton>().InitializeMembers();
                else Tools.GetChildNamed(child, "Fake").SetActive(false);
            }
        }
    }

    public void EnterMenu() {
        transform.parent.GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));

        MapUnit unit = NodeMenu.currentArmy.GetComponent<Army>().GetUnit(UnitSpace.currentUnitPos);
        if (Dissectable(unit)) {
            dissectButton.SetActive(true);
        }
        else {
            dissectButton.SetActive(false);
        }

        GameObject hideButton = Tools.GetChildNamed(gameObject, "Hide Button");
        if (Player.human.GetComponent<Player>().faction != Faction.Noumenon) {
            hideButton.SetActive(false);
        }
        else {
            GameObject costButton = Tools.GetChildNamed(hideButton, "Cost Indicator");
            if (unit == null) Tools.GetChildNamed(costButton, "Cost Text").GetComponent<TextMesh>().text = "";
            else Tools.GetChildNamed(costButton, "Cost Text").GetComponent<TextMesh>().text = (unit.moneyCost / 4).ToString();
        }
    }
    bool Dissectable(MapUnit unit) {
        if (Player.human.GetComponent<Player>().faction == Faction.Paratrophs) {
            if (unit != null) {
                if (unit.faction != Player.human.GetComponent<Player>().faction) {
                    return true;
                }
            }
        }
        return false;
    }

    public void LeaveMenu() {
        nodeMenu.GetComponent<NodeMenu>().LoadArmy();
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
        Player.menuOpen = 1;
    }

    public void MakeUnits(Faction faction, GameObject player) {
        switch (faction) {
            case Faction.Noumenon:
                MakeNoumenon(player);
                break;
            case Faction.Dukkha:
                MakeDukkha(player);
                break;
            case Faction.Paratrophs:
                MakeParatrophs(player);
                break;
            case Faction.Unmar:
                MakeUnmar(player);
                break;
            case Faction.Samata:
                MakeSamata(player);
                break;
            case Faction.Carnot:
                MakeCarnot(player);
                break;
        }
    }

    public void AssignUnits(GameObject player) {
        for (int i =0 ; i < unitSpaces.Length; i++) {
            MapUnit unit = player.GetComponent<Player>().unitBlueprints[i];
            unitSpaces[i].GetComponent<UnitShopSpace>().AddUnit(unit);
        }
    }

    private void MakeNoumenon(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Noumenon, "peon");
        peon.SetHealth(50);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Noumenon, "acolyte");
        acolyte.SetHealth(100);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Noumenon, "shaman");
        shaman.SetHealth(100);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Noumenon, "prelate");
        prelate.SetHealth(200);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeDukkha(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Dukkha, "peon");
        peon.SetHealth(50);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Dukkha, "acolyte");
        acolyte.SetHealth(100);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Dukkha, "shaman");
        shaman.SetHealth(100);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Dukkha, "prelate");
        prelate.SetHealth(200);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeParatrophs(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Paratrophs, "peon");
        peon.SetHealth(30);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Paratrophs, "acolyte");
        acolyte.SetHealth(60);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Paratrophs, "shaman");
        shaman.SetHealth(60);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Paratrophs, "prelate");
        prelate.SetHealth(140);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeUnmar(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Unmar, "peon");
        peon.SetHealth(50);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Unmar, "acolyte");
        acolyte.SetHealth(100);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Unmar, "shaman");
        shaman.SetHealth(100);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Unmar, "prelate");
        prelate.SetHealth(200);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeSamata(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Samata, "peon");
        peon.SetHealth(50);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Samata, "acolyte");
        acolyte.SetHealth(100);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Samata, "shaman");
        shaman.SetHealth(100);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Samata, "prelate");
        prelate.SetHealth(200);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeCarnot(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Carnot, "peon");
        peon.SetHealth(50);
        peon.maxDamage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Carnot, "acolyte");
        acolyte.SetHealth(100);
        acolyte.maxDamage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Carnot, "shaman");
        shaman.SetHealth(100);
        shaman.maxDamage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Carnot, "prelate");
        prelate.SetHealth(200);
        prelate.maxDamage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }
}
