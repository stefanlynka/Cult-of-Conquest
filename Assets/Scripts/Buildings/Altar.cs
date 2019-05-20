using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar{

    public string name;
    public string portrait;
    public int cost;
    public string description;
    public string ability;

    public Altar(string newName, int newCost) {
        name = newName;
        cost = newCost;
        portrait = newName;
    }

    public Altar DeepCopy() {
        Altar copy = new Altar(name, cost);
        copy.portrait = copy.name;
        copy.description = description;
        copy.ability = ability;
        return copy;
    }
}
