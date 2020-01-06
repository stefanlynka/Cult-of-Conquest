using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButton : MonoBehaviour{

    public string buttonType;
    GameObject battleMenu;


    // Start is called before the first frame update
    void Start(){
        battleMenu = transform.parent.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update(){
    }

    private void OnMouseDown() {
        print("Is it AI attacking Human: " + battleMenu.GetComponent<BattleMenu>().AIAttackingHuman());
        if (battleMenu.GetComponent<BattleMenu>().AIAttackingHuman() && buttonType != "back") {
            print("AI is attacking human");
            if (battleMenu.GetComponent<BattleMenu>().AIWantsToFight()) {
                print("AI wants to fight");
                if (buttonType == "instant") battleMenu.GetComponent<BattleMenu>().InstantBattle();
                if (buttonType == "simulation") battleMenu.GetComponent<BattleMenu>().StartSimulation();
            }
            else {
                print("AI retreating");
                battleMenu.GetComponent<BattleMenu>().Retreat();
                battleMenu.GetComponent<BattleMenu>().ExitMenu();
            }
        }
        else {
            if (buttonType == "simulation" && !battleMenu.GetComponent<BattleMenu>().inSimulation && !battleMenu.GetComponent<BattleMenu>().IsBattleOver()) {
                //print("enter simulation");
                battleMenu.GetComponent<BattleMenu>().StartSimulation();
            }
            else if (buttonType == "instant" && !battleMenu.GetComponent<BattleMenu>().inSimulation && !battleMenu.GetComponent<BattleMenu>().IsBattleOver()) {
                //print("enter instant");
                battleMenu.GetComponent<BattleMenu>().InstantBattle();
            }
            else if (buttonType == "retreat" && !battleMenu.GetComponent<BattleMenu>().IsBattleOver()) {
                battleMenu.GetComponent<BattleMenu>().Retreat();
                battleMenu.GetComponent<BattleMenu>().ExitMenu();
            }
            else if (buttonType == "back" && battleMenu.GetComponent<BattleMenu>().IsBattleOver()) {
                battleMenu.GetComponent<BattleMenu>().ExitMenu();
            }
        }

    }
}
