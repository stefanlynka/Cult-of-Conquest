using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSelectionButton : MonoBehaviour{
    Faction faction;
    public static Faction currentFaction;
    Color defaultColour;
    // Start is called before the first frame update
    void Start(){
        currentFaction = Faction.Carnot;
        faction = Tools.StringToFaction(gameObject.name);
        defaultColour = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update(){
        if (currentFaction == faction) GetComponent<SpriteRenderer>().color = Color.grey;
        //else GetComponent<SpriteRenderer>().color = defaultColour;
    }

    private void OnMouseDown() {
        currentFaction = faction;
    }
}
