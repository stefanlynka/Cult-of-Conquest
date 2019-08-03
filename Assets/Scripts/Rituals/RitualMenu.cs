using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualMenu : MonoBehaviour{

    public int test = 0;
    // Start is called before the first frame update
    void Start(){
        GetComponent<Panner>().SetTarget(new Vector3(20, 0, -15));
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        Player.menuOpen = 2;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(20, 0, -15));
        Player.menuOpen = 1;
    }
}
