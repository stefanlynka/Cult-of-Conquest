using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : MonoBehaviour{

    GameObject turnManager;

    // Start is called before the first frame update
    void Start(){
        turnManager = GameObject.Find("/Turn Manager");
    }

    // Update is called once per frame
    void Update(){
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 0) {
            turnManager.GetComponent<TurnManager>().NextTurn();
        }
    }

}
