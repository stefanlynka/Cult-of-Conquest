using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarShopManager : MonoBehaviour{

    GameObject altarSpace;
    Race currentRace;
    GameObject[] altarBuySpaces = new GameObject[4];

    // Start is called before the first frame update
    void Start(){
        InitializeMembers();

        MakeAltars(currentRace);
    }

    // Update is called once per frame
    void Update(){
        
    }

    void InitializeMembers() {
        altarSpace = AltarSpace.currentAltarSpace;
        currentRace = Player.race;
            //NodeMenu.currentNode.GetComponent<Node>().owner;
        GameObject buyingSpaces = Tools.GetChildNamed(gameObject, "Altar Buying Spaces");
        for (int i = 0; i < 4; i++) {
            GameObject buyingSpace = Tools.GetChildNamed(buyingSpaces, "Altar Buying Space " + i);
            altarBuySpaces[i] = buyingSpace;
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

    void MakeAltars(Race race) {
        switch (race) {
            case Race.Noumenon:
                MakeAltar();
                break;
            case Race.Dukkha:
                MakeAltar();
                break;
            case Race.Paratrophs:
                MakeAltar();
                break;
            case Race.Unmar:
                MakeAltar();
                break;
            case Race.Eidalons:
                MakeAltar();
                break;
            case Race.Carnot:
                MakeAltar();
                break;
        }
    }

    void MakeAltar() {
        Altar harvestAltar = new Altar("Harvest", 40);
        harvestAltar.ability = "";
        harvestAltar.description = "Increase this hex's\nincome by 40%";
        altarBuySpaces[0].GetComponent<AltarShopSpace>().SetAltar(harvestAltar);

        Altar devotionAltar = new Altar("Devotion", 50);
        devotionAltar.ability = "";
        devotionAltar.description = "Generate 1 zeal\neach turn";
        altarBuySpaces[1].GetComponent<AltarShopSpace>().SetAltar(devotionAltar);

        Altar conflictAltar = new Altar("Conflict", 60);
        conflictAltar.ability = "";
        conflictAltar.description = "Increase this hex's\ndefense by 20%";
        altarBuySpaces[2].GetComponent<AltarShopSpace>().SetAltar(conflictAltar);

        Altar fateAltar = new Altar("Fate", 70);
        fateAltar.ability = "";
        fateAltar.description = "Increase influence in\ndetermining Oracle";
        altarBuySpaces[3].GetComponent<AltarShopSpace>().SetAltar(fateAltar);
    }
}
