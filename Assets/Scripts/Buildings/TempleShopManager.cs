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
        for (int i = 0; i < TurnManager.players.Count; i++) {
            MakeTempleBlueprints(TurnManager.players[i]);
        }
        GetComponent<Panner>().SetTarget(new Vector3(0, 11, -15));
    }

    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        Player.menuOpen = 2;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 15, -15));
        GameObject.Find("/Node Menu").GetComponent<NodeMenu>().ProphetMenuCheck();
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

    public void MakeTempleBlueprints(GameObject player) {
        Faction faction = player.GetComponent<Player>().faction;
        Temple armamentsTemple = new Temple(TempleName.Armaments, 30, "Decrease unit\ncost by 40%", player.GetComponent<Player>().faction);
        player.GetComponent<Player>().templeBlueprints.Add(TempleName.Armaments, armamentsTemple);
        Temple traditionTemple = new Temple(TempleName.Tradition, 30, "Decrease ritual\ncost at this node", player.GetComponent<Player>().faction);
        player.GetComponent<Player>().templeBlueprints.Add(TempleName.Tradition, traditionTemple);
        Temple protectionTemple = new Temple(TempleName.Protection, 30, "Build a powerful\ndefensive building", player.GetComponent<Player>().faction);
        player.GetComponent<Player>().templeBlueprints.Add(TempleName.Protection, protectionTemple);
        Temple originTemple = new Temple(TempleName.Origin, 50, "Allow creation\nof Prophets", player.GetComponent<Player>().faction);
        player.GetComponent<Player>().templeBlueprints.Add(TempleName.Origin, originTemple);
    }

    void MakeTemple(Faction faction) {
        Temple armamentsTemple = new Temple(TempleName.Armaments, 30, "Decrease unit\ncost by 40%", currentFaction);
        templeBuySpaces[0].GetComponent<TempleShopSpace>().SetTemple(armamentsTemple);

        Temple traditionTemple = new Temple(TempleName.Tradition, 30, "Decrease ritual\ncost at this node", currentFaction);
        templeBuySpaces[1].GetComponent<TempleShopSpace>().SetTemple(traditionTemple);

        Temple protectionTemple = new Temple(TempleName.Protection, 30, "Build a powerful\ndefensive building", currentFaction);
        templeBuySpaces[2].GetComponent<TempleShopSpace>().SetTemple(protectionTemple);

        Temple originTemple = new Temple(TempleName.Origin, 50, "Allow creation\nof Prophets", currentFaction);
        templeBuySpaces[3].GetComponent<TempleShopSpace>().SetTemple(originTemple);
    }
}
