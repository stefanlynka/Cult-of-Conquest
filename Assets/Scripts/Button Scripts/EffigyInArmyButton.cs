using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffigyInArmyButton : MonoBehaviour{

    GameObject nodeMenu;

    // Start is called before the first frame update
    void Start() {
        nodeMenu = GameObject.Find("/Node Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1 && NodeMenu.currentArmy != null && (NodeMenu.currentArmy.GetComponent<Army>().effigy != null || NodeMenu.currentNode.GetComponent<Node>().effigy != null)) {
            Effigy placeholder = NodeMenu.currentArmy.GetComponent<Army>().effigy;
            NodeMenu.currentArmy.GetComponent<Army>().effigy = NodeMenu.currentNode.GetComponent<Node>().effigy;
            NodeMenu.currentNode.GetComponent<Node>().effigy = placeholder;
        }
        /*
        if (Player.menuOpen == 1 && NodeMenu.currentNode != null && NodeMenu.currentNode.GetComponent<Node>().effigy != null && NodeMenu.currentArmy) {
            NodeMenu.currentArmy.GetComponent<Army>().effigy = NodeMenu.currentNode.GetComponent<Node>().effigy;
            NodeMenu.currentNode.GetComponent<Node>().effigy = null;
            nodeMenu.GetComponent<NodeMenu>().LoadEffigy();
        }
        */
    }
}
