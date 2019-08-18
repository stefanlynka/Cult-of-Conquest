using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple {

    public TempleName name;
    public string portrait;
    public int cost;
    public string description;
    public string ability;

    public MapUnit unit;

    public Temple(TempleName newName, int newCost) {
        name = newName;
        cost = newCost;
        portrait = newName.ToString();
        unit = new MapUnit("Temple", Faction.None, "");
        unit.maxHealth = 200;
        unit.currentHealth = 200;
        unit.maxDamage = 50;
        unit.attackSpeed = 100;
    }

    public Temple DeepCopy() {
        Temple copy = new Temple(name, cost);
        copy.portrait = copy.name.ToString();
        copy.description = description;
        copy.ability = ability;
        return copy;
    }
}
