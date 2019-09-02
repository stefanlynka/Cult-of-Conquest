﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarShopManager : MonoBehaviour{

    GameObject altarSpace;
    Faction currentFaction;
    GameObject[] altarBuySpaces = new GameObject[4];

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        
    }

    public void Setup() {
        InitializeMembers();

        MakeAltars(currentFaction);
    }

    void InitializeMembers() {
        altarSpace = AltarSpace.currentAltarSpace;
        currentFaction = Player.human.GetComponent<Player>().faction;
            //NodeMenu.currentNode.GetComponent<Node>().faction;
        GameObject buyingSpaces = Tools.GetChildNamed(gameObject, "Altar Buying Spaces");
        for (int i = 0; i < 4; i++) {
            GameObject buyingSpace = Tools.GetChildNamed(buyingSpaces, "Altar Buying Space " + i);
            altarBuySpaces[i] = buyingSpace;
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

    public void MakeAltars(Faction faction) {
        switch (faction) {
            case Faction.Noumenon:
                MakeAltar();
                break;
            case Faction.Zenteel:
                MakeAltar();
                break;
            case Faction.Paratrophs:
                MakeAltar();
                break;
            case Faction.Unmar:
                MakeAltar();
                break;
            case Faction.Samata:
                MakeAltar();
                break;
            case Faction.Carnot:
                MakeAltar();
                break;
        }
    }

    void MakeAltar() {
        Altar harvestAltar = new Altar(AltarName.Harvest, 20);
        harvestAltar.ability = "";
        harvestAltar.description = "Increase this hex's\nincome by 40%";
        altarBuySpaces[0].GetComponent<AltarShopSpace>().SetAltar(harvestAltar);

        Altar devotionAltar = new Altar(AltarName.Devotion, 20);
        devotionAltar.ability = "";
        devotionAltar.description = "Generate 1 zeal\neach turn";
        altarBuySpaces[1].GetComponent<AltarShopSpace>().SetAltar(devotionAltar);

        Altar conflictAltar = new Altar(AltarName.Conflict, 20);
        conflictAltar.ability = "";
        conflictAltar.description = "Build a powerful\ndefensive building";
        altarBuySpaces[2].GetComponent<AltarShopSpace>().SetAltar(conflictAltar);

        Altar fateAltar = new Altar(AltarName.Fate, 20);
        fateAltar.ability = "";
        fateAltar.description = "Increase influence in\ndetermining Oracle (unimplemented)";
        altarBuySpaces[3].GetComponent<AltarShopSpace>().SetAltar(fateAltar);
    }

    void MakeAltarUnmar() {
        Altar harvestAltar = new Altar(AltarName.Harvest, 99);
        harvestAltar.ability = "";
        harvestAltar.description = "Increase this hex's\nincome by 99%";
        altarBuySpaces[0].GetComponent<AltarShopSpace>().SetAltar(harvestAltar);

        Altar devotionAltar = new Altar(AltarName.Devotion, 99);
        devotionAltar.ability = "";
        devotionAltar.description = "Generate 1 zeal\neach turn";
        altarBuySpaces[1].GetComponent<AltarShopSpace>().SetAltar(devotionAltar);

        Altar conflictAltar = new Altar(AltarName.Conflict, 99);
        conflictAltar.ability = "";
        conflictAltar.description = "Increase this hex's\ndefense by 20%";
        altarBuySpaces[2].GetComponent<AltarShopSpace>().SetAltar(conflictAltar);

        Altar fateAltar = new Altar(AltarName.Fate, 99);
        fateAltar.ability = "";
        fateAltar.description = "Increase influence in\ndetermining Oracle";
        altarBuySpaces[3].GetComponent<AltarShopSpace>().SetAltar(fateAltar);
    }
}
