using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonUpgrade : MonoBehaviour
{

    GameObject upgradeMenu;
    // Start is called before the first frame update

    void Start() {
        upgradeMenu = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        upgradeMenu.GetComponent<UpgradeMenu>().ExitMenu();
    }
}
