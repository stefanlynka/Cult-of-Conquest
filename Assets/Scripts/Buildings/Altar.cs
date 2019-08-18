using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar{

    public AltarName name;
    public string portrait;
    public int cost;
    public string description;
    public string ability;

    public MapUnit unit;

    public Altar(AltarName newName, int newCost) {
        name = newName;
        cost = newCost;
        portrait = newName.ToString();
        unit = new MapUnit("altar", Faction.None, "");
        unit.maxHealth = 100;
        unit.currentHealth = 100;
        unit.maxDamage = 10;
        unit.attackSpeed = 20;
    }

    public Altar DeepCopy() {
        Altar copy = new Altar(name, cost);
        copy.portrait = copy.name.ToString();
        copy.description = description;
        copy.ability = ability;
        return copy;
    }
}
