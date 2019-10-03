using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSelectionButton : MonoBehaviour{
    Faction faction;
    public static Faction currentFaction;
    // Start is called before the first frame update
    void Start(){
        currentFaction = Faction.Carnot;
        faction = Tools.StringToFaction(gameObject.name);
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        currentFaction = faction;
    }
}
