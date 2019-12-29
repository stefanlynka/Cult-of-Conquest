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
    int defaultHealth = 200;
    int defaultDamage = 30;
    int defaultSpeed = 100;

    public Temple(TempleName newName, int newCost, string desc, Faction newFaction) {
        name = newName;
        cost = newCost;
        description = desc;
        portrait = newName.ToString();
        faction = newFaction;
        MakeUnit();

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
        MakeUnit();
    }

    public void MakeUnit() {
        unit = new MapUnit("Temple", Faction.None, "");
        unit.maxHealth = defaultHealth;
        unit.currentHealth = defaultHealth;
        unit.attackSpeed = defaultSpeed;
        if (name == TempleName.Protection) unit.SetDamage(defaultDamage);
        else unit.SetDamage(0);
    }
}
