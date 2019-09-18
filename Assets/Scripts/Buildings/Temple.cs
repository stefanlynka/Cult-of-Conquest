using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple {

    public TempleName name;
    public string portrait;
    public int cost;
    public string description;
    public Faction faction;
    public MapUnit unit;

    public Temple(TempleName newName, int newCost, string desc, Faction newFaction) {
        name = newName;
        cost = newCost;
        description = desc;
        portrait = newName.ToString();
        faction = newFaction;
        unit = new MapUnit("Temple", Faction.None, "");
        unit.maxHealth = 200;
        unit.currentHealth = 200;
        unit.maxDamage = 30;
        unit.attackSpeed = 300;
        unit.Reset();
    }

    public Temple DeepCopy() {
        Temple newTemple = new Temple(name, cost, description, faction);
        newTemple.portrait = portrait;
        newTemple.faction = faction;
        newTemple.unit = unit.DeepCopy();
        return newTemple;
    }

    public void ResetUnit() {
        if (unit == null) {
            unit = new MapUnit("Temple", Faction.None, "");
            unit.maxHealth = 200;
            unit.currentHealth = unit.maxHealth / 2;
            unit.maxDamage = 50;
            unit.attackSpeed = 100;
        }
    }
    public void SetNull() {
        name = TempleName.None;
        cost = 0;
        description = "";
        portrait = "";
        unit = null;
    }
}
