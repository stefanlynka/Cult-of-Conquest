using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple {

    public TempleName name;
    public string portrait;
    public int cost;
    public string description;
    public string ability;
    public Faction faction;

    public MapUnit unit;

    public Temple(TempleName newName, int newCost, Faction newFaction) {
        name = newName;
        cost = newCost;
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
        Temple copy = new Temple(name, cost, faction);
        copy.portrait = copy.name.ToString();
        copy.description = description;
        copy.ability = ability;
        return copy;
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
}
