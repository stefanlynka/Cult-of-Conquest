using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFactionButton : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){
        transform.parent.GetComponent<Panner>().SetTarget(new Vector3(0, 0, -30));
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        Faction faction = FactionSelectionButton.currentFaction;
        GameObject.Find("/Game Controller").GetComponent<Controller>().PostFactionStartup(faction);
        transform.parent.GetComponent<Panner>().SetTarget(new Vector3(-20, 0, -30));
    }
}
