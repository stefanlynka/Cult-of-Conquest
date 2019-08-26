using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleShopManager : MonoBehaviour{

    GameObject templeSpace;
    Faction currentFaction;
    GameObject[] templeBuySpaces = new GameObject[4];

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Setup() {
        InitializeMembers();

        MakeTemples(currentFaction);
    }

    void InitializeMembers() {
        templeSpace = TempleSpace.currentTempleSpace;
        currentFaction = Player.human.GetComponent<Player>().faction;
        //NodeMenu.currentNode.GetComponent<Node>().faction;
        GameObject buyingSpaces = Tools.GetChildNamed(gameObject, "Temple Buying Spaces");
        for (int i = 0; i < 4; i++) {
            GameObject buyingSpace = Tools.GetChildNamed(buyingSpaces, "Temple Buying Space " + i);
            templeBuySpaces[i] = buyingSpace;
        }
        GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
    }

    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        Player.menuOpen = 2;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 15, -15));
        Player.menuOpen = 1;
    }

    public void MakeTemples(Faction faction) {
        switch (faction) {
            case Faction.Noumenon:
                MakeTemple(faction);
                break;
            case Faction.Zenteel:
                MakeTemple(faction);
                break;
            case Faction.Paratrophs:
                MakeTemple(faction);
                break;
            case Faction.Unmar:
                MakeTemple(faction);
                break;
            case Faction.Samata:
                MakeTemple(faction);
                break;
            case Faction.Carnot:
                MakeTemple(faction);
                break;
        }
    }

    void MakeTemple(Faction faction) {
        Temple armamentsTemple = new Temple(TempleName.Armaments, 40, faction);
        armamentsTemple.ability = "";
        armamentsTemple.description = "Decrease unit\ncost by 40%";
        templeBuySpaces[0].GetComponent<TempleShopSpace>().SetTemple(armamentsTemple);

        Temple traditionTemple = new Temple(TempleName.Tradition, 50, faction);
        traditionTemple.ability = "";
        traditionTemple.description = "Decrease ritual\npreparation time";
        templeBuySpaces[1].GetComponent<TempleShopSpace>().SetTemple(traditionTemple);

        Temple protectionTemple = new Temple(TempleName.Protection, 60, faction);
        protectionTemple.ability = "";
        protectionTemple.description = "Increase this hex's\ndefense by 20%";
        templeBuySpaces[2].GetComponent<TempleShopSpace>().SetTemple(protectionTemple);

        Temple originTemple = new Temple(TempleName.Origin, 70, faction);
        originTemple.ability = "";
        originTemple.description = "Allow creation\nof Prophets";
        templeBuySpaces[3].GetComponent<TempleShopSpace>().SetTemple(originTemple);
    }
}
