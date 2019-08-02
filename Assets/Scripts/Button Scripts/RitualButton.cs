using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualButton : MonoBehaviour{
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            GameObject ritualMenu = GameObject.Find("/Ritual Menu");
            ritualMenu.GetComponent<RitualMenu>().EnterMenu();
            Player.menuOpen = 2;
        }
    }
}
