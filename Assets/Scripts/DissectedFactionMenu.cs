using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissectedFactionMenu : MonoBehaviour{

    public string buildingType;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void EnterMenu(string building) {
        buildingType = building;
        Player.menuOpen = 2;
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
    }
    public void ExitMenu() {
        Player.menuOpen = 1;
        GetComponent<Panner>().SetTarget(new Vector3(0, -23, -15));
    }

}
