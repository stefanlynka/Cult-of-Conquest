using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpace : MonoBehaviour
{
    GameObject unitBuyingMenu;
    public static bool buyingMenuOpen = false;
    public static GameObject currentUnitSpace;
    public static unitPos currentUnitPos;

    public int position;
    public bool frontRow;

    // Start is called before the first frame update
    void Start(){
        unitBuyingMenu = GameObject.Find("/Unit Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }
    private void OnMouseDown() {
        //if (!buyingMenuOpen) {
        unitBuyingMenu.GetComponent<Panner>().SetTarget(new Vector3(0, 0, -15));
        currentUnitSpace = gameObject;
        currentUnitPos = new unitPos(position, frontRow);
        //buyingMenuOpen = true;
        /*
        }
        else {
            unitBuyingMenu.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
            currentUnitSpace = null;
            buyingMenuOpen = false;
        }
        */
    }

}
public struct unitPos{
    public int position;
    public bool frontRow;
    public unitPos(int pos, bool row) {
        position = pos;
        frontRow = row;
    }
}
