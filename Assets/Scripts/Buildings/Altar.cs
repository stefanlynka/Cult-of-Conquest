using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar{

    public AltarName name;
    public string portrait;
    public int cost;
    public string description;
    int defaultHealth = 150;
    int defaultDamage = 15;
    int defaultSpeed = 60;

    public MapUnit unit;

    public Altar(AltarName newName, int newCost, string desc) {
        name = newName;
        cost = newCost;
        description = desc;
        portrait = newName.ToString();
        MakeUnit();

        unit.Reset();
    }

    public Altar DeepCopy() {
        Altar newAltar = new Altar(name, cost, description);
        newAltar.portrait = portrait;
        newAltar.unit = unit.DeepCopy();
        return newAltar;
    }


    public void ResetUnit() {
        MakeUnit();
    }

    public void MakeUnit() {
        unit = new MapUnit("Altar", Faction.None, "");
        unit.maxHealth = defaultHealth;
        unit.currentHealth = defaultHealth;
        unit.attackSpeed = defaultSpeed;
        if (name == AltarName.Conflict) unit.SetDamage(defaultDamage);
        else unit.SetDamage(0);
    }
}
