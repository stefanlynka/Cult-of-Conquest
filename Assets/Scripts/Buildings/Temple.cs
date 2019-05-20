using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple {

    public string name;
    public string portrait;
    public int cost;
    public string description;
    public string ability;

    public Temple(string newName, int newCost) {
        name = newName;
        cost = newCost;
        portrait = newName;
    }

    public Temple DeepCopy() {
        Temple copy = new Temple(name, cost);
        copy.portrait = copy.name;
        copy.description = description;
        copy.ability = ability;
        return copy;
    }
}
