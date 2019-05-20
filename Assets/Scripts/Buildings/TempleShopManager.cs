using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleShopManager : MonoBehaviour{

    GameObject templeSpace;
    Race currentRace;
    GameObject[] templeBuySpaces = new GameObject[4];

    // Start is called before the first frame update
    void Start() {
        InitializeMembers();

        MakeTemples(currentRace);
    }

    // Update is called once per frame
    void Update() {

    }

    void InitializeMembers() {
        templeSpace = TempleSpace.currentTempleSpace;
        currentRace = Player.race;
        //NodeMenu.currentNode.GetComponent<Node>().owner;
        GameObject buyingSpaces = Tools.GetChildNamed(gameObject, "Temple Buying Spaces");
        for (int i = 0; i < 4; i++) {
            GameObject buyingSpace = Tools.GetChildNamed(buyingSpaces, "Temple Buying Space " + i);
            templeBuySpaces[i] = buyingSpace;
        }
        GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
    }

    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        Player.menuOpen = true;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 15, -15));
        Player.menuOpen = false;
    }

    void MakeTemples(Race race) {
        switch (race) {
            case Race.Noumenon:
                MakeTemple();
                break;
            case Race.Dukkha:
                MakeTemple();
                break;
            case Race.Paratrophs:
                MakeTemple();
                break;
            case Race.Unmar:
                MakeTemple();
                break;
            case Race.Eidalons:
                MakeTemple();
                break;
            case Race.Carnot:
                MakeTemple();
                break;
        }
    }

    void MakeTemple() {
        Temple armamentsTemple = new Temple("Armaments", 40);
        armamentsTemple.ability = "";
        armamentsTemple.description = "Decrease unit\ncost by 40%";
        templeBuySpaces[0].GetComponent<TempleShopSpace>().SetTemple(armamentsTemple);

        Temple traditionTemple = new Temple("Tradition", 50);
        traditionTemple.ability = "";
        traditionTemple.description = "Decrease ritual\npreparation time";
        templeBuySpaces[1].GetComponent<TempleShopSpace>().SetTemple(traditionTemple);

        Temple protectionTemple = new Temple("Protection", 60);
        protectionTemple.ability = "";
        protectionTemple.description = "Increase this hex's\ndefense by 20%";
        templeBuySpaces[2].GetComponent<TempleShopSpace>().SetTemple(protectionTemple);

        Temple originTemple = new Temple("Origin", 70);
        originTemple.ability = "";
        originTemple.description = "Allow creation\nof Prophets";
        templeBuySpaces[3].GetComponent<TempleShopSpace>().SetTemple(originTemple);
    }
}
