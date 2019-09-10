using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualButton : MonoBehaviour{

    GameObject ritualMenu, readySprite;

    // Start is called before the first frame update
    void Start() {
        ritualMenu = GameObject.Find("/Ritual Menu");
        readySprite = Tools.GetChildNamed(gameObject, "Ritual Indicator Sprite");

    }

    // Update is called once per frame
    void Update(){
        UpdateSprite();
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            Ritual ritual = NodeMenu.currentNode.GetComponent<Node>().ritual;

            if (ritual.IsReady()) {
                SelectNodesForRitual();
            }
            else if (NodeMenu.currentNode.GetComponent<Node>().owner && NodeMenu.currentNode.GetComponent<Node>().owner==Player.human) {
                ritualMenu.GetComponent<RitualMenu>().EnterMenu();
            }
        }
    }

    void SelectNodesForRitual() {
        print("Selecting time");
        ritualMenu.GetComponent<RitualManager>().SetupRitualSelection(NodeMenu.currentNode.GetComponent<Node>().ritual, NodeMenu.currentNode);
        CloseMenu();
    }
    void CloseMenu() {
        GameObject.Find("/Node Menu").GetComponent<NodeMenu>().ExitMenu();
    }

    void UpdateSprite() {
        if (NodeMenu.currentNode) {
            Ritual currentRitual = NodeMenu.currentNode.GetComponent<Node>().ritual;

            if (currentRitual.IsReady()) readySprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Ritual3");
            else if (!currentRitual.IsEmpty()) readySprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Ritual1");
            else readySprite.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
