using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualSlot : MonoBehaviour{

    public Ritual ritualBlueprint;
    public GameObject human;

    // Start is called before the first frame update
    void Start(){
        human = Player.human;
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 2) {
            if (human.GetComponent<Player>().BuyRitual(ritualBlueprint)) {
                transform.parent.GetComponent<RitualMenu>().ExitMenu();
            }
        }
    }
}
