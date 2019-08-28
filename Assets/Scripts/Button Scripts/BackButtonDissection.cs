using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonDissection : MonoBehaviour{

    GameObject dissectedMenu;
    // Start is called before the first frame update

    void Start() {
        dissectedMenu = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown() {
        dissectedMenu.GetComponent<DissectedFactionMenu>().ExitMenu();
    }
}
