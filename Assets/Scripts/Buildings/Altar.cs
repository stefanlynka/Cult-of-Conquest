using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar{

    public AltarName name;
    public string portrait;
    public int cost;
    public string description;

    public MapUnit unit;

    public Altar(AltarName newName, int newCost, string desc) {
        name = newName;
        cost = newCost;
        description = desc;
        portrait = newName.ToString();
        unit = new MapUnit("Altar", Faction.None, "");
        unit.maxHealth = 150;
        unit.currentHealth = 150;
        unit.maxDamage = 10;
        unit.attackSpeed = 60;
        unit.Reset();
    }

    public Altar DeepCopy() {
        Altar newAltar = new Altar(name, cost, description);
        newAltar.portrait = portrait;
        newAltar.unit = unit.DeepCopy();
        return newAltar;
    }


    public void ResetUnit() {
        unit = new MapUnit("Altar", Faction.None, "");
        unit.maxHealth = 100;
        unit.currentHealth = 100;
        unit.maxDamage = 10;
        unit.attackSpeed = 20;
    }

    public void SetNull() {
        name = AltarName.None;
        cost = 0;
        description = "";
        portrait = "";
        unit = null;
    }
}
