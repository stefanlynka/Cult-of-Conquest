using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour{

    List<GameObject> players = new List<GameObject>();
    GameObject nodeManager, playerMenu;

    // Start is called before the first frame update
    void Start(){
        FindMembers();
        CallStartupFunctions();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void FindMembers() {
        nodeManager = GameObject.Find("/Node Manager");
        playerMenu = GameObject.Find("/Players");
        for (int i = 0; i < playerMenu.transform.childCount; i++) {
            GameObject child = playerMenu.transform.GetChild(i).gameObject;
            if (child.GetComponent<Player>()) players.Add(child);
        }
    }
    void CallStartupFunctions() {
        nodeManager.GetComponent<NodeManager>().Startup();
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            player.GetComponent<Player>().Startup();
        }
    }
}
