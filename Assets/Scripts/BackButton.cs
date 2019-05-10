using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    GameObject unitShop;
    // Start is called before the first frame update
    void Start(){
        unitShop = GameObject.Find("/Unit Buying Menu");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        unitShop.GetComponent<Panner>().SetTarget(new Vector3(0, 21, -15));
    }
}
