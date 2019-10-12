using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShopManager : MonoBehaviour {

    GameObject army, nodeMenu, unitShop, dissectButton, hideButton;
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
        GameObject otherButtons = Tools.GetChildNamed(transform.parent.gameObject, "Other Buttons");
        dissectButton =  Tools.GetChildNamed(otherButtons, "Dissect Button");
        hideButton = Tools.GetChildNamed(otherButtons, "Hide Button");

        //army = NodeMenu.currentArmy;
        //if (army) currentFaction = army.GetComponent<Army>().faction;
        List<GameObject> players = Controller.players;
        
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            MakeUnits(player.GetComponent<Player>().faction, player);
        }
        AssignUnits(Player.human);

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

        if (Player.human.GetComponent<Player>().faction != Faction.Noumenon) {
            hideButton.SetActive(false);
        }
        else {
            GameObject costButton = Tools.GetChildNamed(hideButton, "Cost Indicator");
            if (unit == null) Tools.GetChildNamed(costButton, "Cost Text").GetComponent<TextMesh>().text = "";
            else Tools.GetChildNamed(costButton, "Cost Text").GetComponent<TextMesh>().text = (unit.moneyCost / 4).ToString();
        }
        for(int i=0; i< unitSpaces.Length; i++) {
            GameObject unitSpace = unitSpaces[i];
            if (unitSpace) {
                unitSpace.GetComponent<UnitShopSpace>().UpdateUnit();
            }
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
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
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
        peon.SetHealth(25);
        peon.maxDamage = 5;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Noumenon, "acolyte");
        acolyte.SetHealth(40);
        acolyte.maxDamage = 9;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Noumenon, "shaman");
        shaman.SetHealth(50);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Noumenon, "prelate");
        prelate.SetHealth(60);
        prelate.maxDamage = 20;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeDukkha(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Dukkha, "peon");
        peon.SetHealth(25);
        peon.maxDamage = 5;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Dukkha, "acolyte");
        acolyte.SetHealth(40);
        acolyte.maxDamage = 9;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Dukkha, "shaman");
        shaman.SetHealth(50);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Dukkha, "prelate");
        prelate.SetHealth(60);
        prelate.maxDamage = 20;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeParatrophs(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Paratrophs, "peon");
        peon.SetHealth(15);
        peon.maxDamage = 5;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Paratrophs, "acolyte");
        acolyte.SetHealth(25);
        acolyte.maxDamage = 10;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Paratrophs, "shaman");
        shaman.SetHealth(50);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Paratrophs, "prelate");
        prelate.SetHealth(60);
        prelate.maxDamage = 20;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeUnmar(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Unmar, "peon");
        peon.SetHealth(25);
        peon.maxDamage = 5;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10; 
        peon.maxShield = 1;
        peon.currentShield = peon.maxShield;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Unmar, "acolyte");
        acolyte.SetHealth(40);
        acolyte.maxDamage = 9;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.maxShield = 2;
        acolyte.currentShield = acolyte.maxShield;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Unmar, "shaman");
        shaman.SetHealth(50);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.maxShield = 3;
        shaman.currentShield = shaman.maxShield;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Unmar, "prelate");
        prelate.SetHealth(60);
        prelate.maxDamage = 20;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25; 
        prelate.maxShield = 3;
        prelate.currentShield = prelate.maxShield;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeSamata(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Samata, "peon");
        peon.SetHealth(40);
        peon.maxDamage = 3;
        peon.attackRange = 1;
        peon.attackSpeed = 55;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Samata, "acolyte");
        acolyte.SetHealth(40);
        acolyte.maxDamage = 9;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Samata, "shaman");
        shaman.SetHealth(40);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Samata, "prelate");
        prelate.SetHealth(40);
        prelate.maxDamage = 30;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeCarnot(GameObject player) {
        MapUnit peon = new MapUnit("peon", Faction.Carnot, "peon");
        peon.SetHealth(25);
        peon.maxDamage = 5;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        peon.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Faction.Carnot, "acolyte");
        acolyte.SetHealth(40);
        acolyte.maxDamage = 9;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 120;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        acolyte.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Faction.Carnot, "shaman");
        shaman.SetHealth(50);
        shaman.maxDamage = 12;
        shaman.attackRange = 3;
        shaman.attackSpeed = 180;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        shaman.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Faction.Carnot, "prelate");
        prelate.SetHealth(60);
        prelate.maxDamage = 20;
        prelate.attackRange = 2;
        prelate.attackSpeed = 240;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        prelate.Reset();
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }
}
