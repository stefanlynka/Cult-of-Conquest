using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour{


    // Start is called before the first frame update
    void Start() {
        GetComponent<Panner>().SetTarget(new Vector3(0, -12, -15));
    }

    // Update is called once per frame
    void Update() {

    }
    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        Player.menuOpen = 1;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, -12, -15));
        Player.menuOpen = 0;
    }
}
