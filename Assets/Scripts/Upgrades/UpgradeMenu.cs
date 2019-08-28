using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour{


    // Start is called before the first frame update
    void Start() {
        GetComponent<Panner>().SetTarget(new Vector3(0, -12, -5));
    }

    // Update is called once per frame
    void Update() {

    }
    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -5));
        Player.menuOpen = 1;
    }
    public void ExitMenu() {
        if (Player.human.GetComponent<Player>().upgradesBackup.Count != 0) {
            print("Backups in use, Clear and reset");
        }
        GetComponent<Panner>().SetTarget(new Vector3(0, -12, -5));
        Player.menuOpen = 0;
    }
}
