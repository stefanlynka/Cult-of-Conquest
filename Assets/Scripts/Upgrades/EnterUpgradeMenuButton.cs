using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterUpgradeMenuButton : MonoBehaviour
{
    GameObject upgradeMenuButton;

    // Start is called before the first frame update
    void Start(){
        upgradeMenuButton = GameObject.Find("/Upgrade Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }
    private void OnMouseDown() {
        if (Player.menuOpen==0) upgradeMenuButton.GetComponent<UpgradeMenu>().EnterMenu();
    }
}
