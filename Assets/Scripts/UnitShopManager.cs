using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShopManager : MonoBehaviour {

    GameObject army;
    Race currentRace;
    int unitSpaceCount = 4;
    GameObject[] unitSpaces;

    // Start is called before the first frame update
    void Start() {
        initializeMembers();

    }

    // Update is called once per frame
    void Update() {

    }

    public void initializeMembers() {
        unitSpaces = new GameObject[unitSpaceCount];
        for (int i = 0; i < transform.childCount; i++) {
            GameObject unitSpace = Tools.GetChildNamed(gameObject, "Buy Unit Space " + i);
            if (unitSpace) unitSpaces[i] = unitSpace;
        }

        currentRace = TurnManager.human.GetComponent<Player>().race;

        transform.parent.gameObject.GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
        //army = NodeMenu.currentArmy;
        //if (army) currentRace = army.GetComponent<Army>().race;
        List<GameObject> players = Controller.players;
        for (int i = 0;i < players.Count; i++) {
            GameObject player = players[i];
            MakeUnits(player.GetComponent<Player>().race, player);
            if (!player.GetComponent<AI>()) AssignUnits(player);
        }
    }

    public void MakeUnits(Race race, GameObject player) {
        switch (race) {
            case Race.Noumenon:
                MakeNoumenon(player);
                break;
            case Race.Dukkha:
                MakeDukkha(player);
                break;
            case Race.Paratrophs:
                MakeParatrophs(player);
                break;
            case Race.Unmar:
                MakeUnmar(player);
                break;
            case Race.Eidalons:
                MakeEidalons(player);
                break;
            case Race.Carnot:
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
        MapUnit peon = new MapUnit("peon", Race.Noumenon, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Noumenon, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Noumenon, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Noumenon, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeDukkha(GameObject player) {
        MapUnit peon = new MapUnit("peon", Race.Dukkha, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Dukkha, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Dukkha, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Dukkha, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeParatrophs(GameObject player) {
        MapUnit peon = new MapUnit("peon", Race.Paratrophs, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Paratrophs, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Paratrophs, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Paratrophs, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeUnmar(GameObject player) {
        MapUnit peon = new MapUnit("peon", Race.Unmar, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Unmar, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Unmar, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Unmar, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeEidalons(GameObject player) {
        MapUnit peon = new MapUnit("peon", Race.Eidalons, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Eidalons, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Eidalons, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Eidalons, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }

    private void MakeCarnot(GameObject player) {
        MapUnit peon = new MapUnit("peon", Race.Carnot, "peon");
        peon.SetHealth(50);
        peon.damage = 10;
        peon.attackRange = 1;
        peon.attackSpeed = 60;
        peon.moneyCost = 5;
        peon.zealCost = 0;
        peon.power = 10;
        player.GetComponent<Player>().unitBlueprints.Add(peon.DeepCopy());
        //unitSpaces[0].GetComponent<UnitShopSpace>().AddUnit(peon);

        MapUnit acolyte = new MapUnit("acolyte", Race.Carnot, "acolyte");
        acolyte.SetHealth(100);
        acolyte.damage = 20;
        acolyte.attackRange = 1;
        acolyte.attackSpeed = 100;
        acolyte.moneyCost = 10;
        acolyte.zealCost = 0;
        acolyte.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(acolyte.DeepCopy());
        //unitSpaces[1].GetComponent<UnitShopSpace>().AddUnit(acolyte);

        MapUnit shaman = new MapUnit("shaman", Race.Carnot, "shaman");
        shaman.SetHealth(100);
        shaman.damage = 10;
        shaman.attackRange = 3;
        shaman.attackSpeed = 120;
        shaman.moneyCost = 10;
        shaman.zealCost = 1;
        shaman.power = 15;
        player.GetComponent<Player>().unitBlueprints.Add(shaman.DeepCopy());
        //unitSpaces[2].GetComponent<UnitShopSpace>().AddUnit(shaman);

        MapUnit prelate = new MapUnit("prelate", Race.Carnot, "prelate");
        prelate.SetHealth(200);
        prelate.damage = 40;
        prelate.attackRange = 2;
        prelate.attackSpeed = 120;
        prelate.moneyCost = 20;
        prelate.zealCost = 0;
        prelate.power = 25;
        player.GetComponent<Player>().unitBlueprints.Add(prelate.DeepCopy());
        //unitSpaces[3].GetComponent<UnitShopSpace>().AddUnit(prelate);
    }
}
