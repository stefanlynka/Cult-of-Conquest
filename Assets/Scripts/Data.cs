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
    Spending1,
    Attacks1,
    Spending2,
    Attacks2,
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
public enum OptionType {
    Prophet,
    Unit,
    Upgrade,
    Replace,
    Replenish,
    Temple,
    Altar,
    None
}
public class AllocateOption {
    public GameObject node;
    public GameObject army;
    public OptionType type;
    public float utility;
    public int unitIndex;
    public TempleName templeName;
    public AltarName altarName;
    public int cost;
    public UnitPos unitPos;
    public int oldUnitIndex;
    public AllocateOption(OptionType prophetOption, float newUtility, int newCost) {
        node = null;
        army = null;
        type = prophetOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = 0;
        oldUnitIndex = 0;
        unitPos = new UnitPos();
        templeName = TempleName.None;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType unitOption, float newUtility, int newCost, GameObject newArmy, int mapunitIndex) {
        node = null;
        army = newArmy;
        type = unitOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = mapunitIndex;
        oldUnitIndex = 0;
        unitPos = new UnitPos();
        templeName = TempleName.None;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType upgradeOption, float newUtility, int newCost, GameObject newArmy, int mapunitIndex, int oldIndex) {
        node = null;
        army = newArmy;
        type = upgradeOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = mapunitIndex;
        oldUnitIndex = oldIndex;
        unitPos = new UnitPos();
        templeName = TempleName.None;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType replaceOption, float newUtility, int newCost, GameObject newArmy, int mapunitIndex, UnitPos position) {
        node = null;
        army = newArmy;
        type = replaceOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = mapunitIndex;
        unitPos = position;
        oldUnitIndex = 0;
        templeName = TempleName.None;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType replenishOption, float newUtility, int newCost, GameObject armyToReplenish) {
        node = null;
        army = armyToReplenish;
        type = replenishOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = 0;
        oldUnitIndex = 0;
        unitPos = new UnitPos();
        templeName = TempleName.None;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType templeOption, float newUtility, int newCost, GameObject buildingNode, TempleName newname) {
        node = buildingNode;
        army = null;
        type = templeOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = 0;
        oldUnitIndex = 0;
        unitPos = new UnitPos();
        templeName = newname;
        altarName = AltarName.None;
    }
    public AllocateOption(OptionType altarOption, float newUtility, int newCost, GameObject buildingNode, AltarName newname) {
        node = buildingNode;
        army = null;
        type = altarOption;
        utility = newUtility;
        cost = newCost;
        unitIndex = 0;
        oldUnitIndex = 0;
        unitPos = new UnitPos();
        templeName = TempleName.None;
        altarName = newname;
    }
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


