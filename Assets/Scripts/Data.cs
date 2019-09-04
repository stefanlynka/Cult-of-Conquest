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
    Zenteel,
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
    public ListGameObject Activate;
    public delegate void ListGameObject(List<GameObject> targets);

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
    public Ritual DeepCopy() {
        Ritual newRitual = new Ritual();
        newRitual.zealCost = zealCost;
        newRitual.prepTime = prepTime;
        newRitual.name = name;
        newRitual.description = description;
        newRitual.range = range;
        newRitual.numTargets = numTargets;
        newRitual.Activate = Activate;
        return newRitual;
    }
}

public class Upgrade {
    public string name;
    public string description;
    public int maxLevel;
    public int currentLevel;
    public int zealCost;

    public Upgrade(string newName, int cost, int levelCap, string desc) {
        name = newName;
        description = desc;
        maxLevel = levelCap;
        currentLevel = 0;
        zealCost = cost;
    }
    public Upgrade DeepCopy() {
        Upgrade newUpgrade = new Upgrade(name, zealCost, maxLevel, description);
        return newUpgrade;
    }
}

public class Effigy {
    public Faction faction;
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
    public Intent(GameObject army, GameObject node) {
        armyMoving = army; 
        targetNode = node;
    }
    public GameObject targetNode;
    public GameObject armyMoving;
}
public enum TurnPhase {
    DefendAgainstThreats,
    Scouting,
    Allocating,
    Attacks,
    Done
}
public struct ArmyInfo {
    public ArmyInfo(int time, float power) {
        timeSinceScout = time;
        expectedPower = power;
    }
    public int timeSinceScout;
    public float expectedPower;
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


