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

public enum Faction {
    Noumenon,
    Dukkha,
    Paratrophs,
    Unmar,
    Samata,
    Carnot,
    Independent,
    None
}

public struct Ritual {
    public int zealCost;
    public int prepTime;
    public string name;
    public string description;
    public int range;
    public int numTargets;
    public delegate void ListGameObject(List<GameObject> targets);
    public ListGameObject Activate;
    public Ritual(string newName, int cost, int time, string desc, int newRange, int newTargets, ListGameObject ability) {
        zealCost = cost;
        prepTime = time;
        name = newName;
        description = desc;
        range = newRange;
        numTargets = newTargets;
        Activate = ability;
    }
    public void Clear() {
        zealCost = 0;
        prepTime = 0;
        name = "";
        description = "";
        range = 0;
        numTargets = 0;
    }
    public bool IsEmpty() {
        if (name == null || name == "") return true;
        return false;
    }
    public bool IsReady() {
        if (name != null && name != "" && prepTime <= 0) return true;
        return false;
    }
}


// For Option
public struct Option {
    public OptionName optionName;
    public GameObject army;
    public MapUnit unit;
    public GameObject targetNode;
}

public enum OptionName {
    BuildUnit,
    AttackNode
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

// For AI
public struct Intent {
    public string type;
    public GameObject army;
    public GameObject node;
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

public enum TempleName {
    Protection,
    Armaments,
    Tradition,
    Origin,
    None
}
public enum AltarName {
    Harvest,
    Devotion,
    Conflict,
    Fate,
    Armaments,
    None
}


