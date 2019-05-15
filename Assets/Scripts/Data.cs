using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data{
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}

public enum Race {
    Noumenon,
    Dukkha,
    Paratrophs,
    Unmar,
    Eidalons,
    Carnot,
    Independent,
    None
}

// For BattleMenu
public struct Cooldown {
    public int timeToAct;
    public MapUnit unit;
    public string side;
    public Cooldown(int time, MapUnit newUnit, string allegiance) {
        timeToAct = time;
        unit = newUnit;
        side = allegiance;
    }

}
public enum BattleType {
    simulation,
    instant,
    rts
}

// For UnitSpace
public struct UnitPos {
    public int position;
    public bool frontRow;
    public UnitPos(int pos, bool row) {
        position = pos;
        frontRow = row;
    }
}


// For Node
public enum Altar {
    Harvest,
    Devotion,
    Conflict,
    Fate,
    None
}
public enum Church {
    Protection,
    Armaments,
    Tradition,
    Origin,
    None
}

