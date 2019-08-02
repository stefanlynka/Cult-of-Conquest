using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpace : MonoBehaviour
{
    GameObject unitBuyingMenu;
    public static bool buyingMenuOpen = false;
    public static GameObject currentUnitSpace;
    public static UnitPos currentUnitPos;

    public int position;
    public bool frontRow;
    public bool shop = false;

    // Start is called before the first frame update
    void Start(){
        unitBuyingMenu = GameObject.Find("/Unit Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (shop) {
            //print("occupied = " + NodeMenu.currentNode.GetComponent<Node>().occupied);
            if (NodeMenu.currentNode.GetComponent<Node>().occupied) {
                unitBuyingMenu.GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
                currentUnitSpace = gameObject;
                currentUnitPos = new UnitPos(position, frontRow);
                Player.menuOpen = 2;
            }
        }
    }

}

