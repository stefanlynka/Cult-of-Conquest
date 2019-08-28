using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissectedFactionMenu : MonoBehaviour{

    public string situation;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void EnterMenu(string topic) {
        situation = topic;
        Player.menuOpen = 2;
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -10));
    }
    public void ExitMenu() {
        Player.menuOpen = 1;
        GetComponent<Panner>().SetTarget(new Vector3(0, -23, -10));
    }

}
