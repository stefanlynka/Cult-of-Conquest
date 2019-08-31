using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffigyInNodeButton : MonoBehaviour{

    GameObject nodeMenu;

    // Start is called before the first frame update
    void Start(){
        nodeMenu = GameObject.Find("/Node Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1 && NodeMenu.currentArmy!= null && NodeMenu.currentArmy.GetComponent<Army>().effigy != null) {
            NodeMenu.currentNode.GetComponent<Node>().effigy = NodeMenu.currentArmy.GetComponent<Army>().effigy;
            NodeMenu.currentArmy.GetComponent<Army>().effigy = null;
            nodeMenu.GetComponent<NodeMenu>().LoadEffigy();
        }
    }
}
