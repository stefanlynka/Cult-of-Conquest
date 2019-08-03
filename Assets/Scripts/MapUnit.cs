using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit
{
    public string name;
    public Race race = Race.Independent;
    public string portraitName = "zergling";

    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public int attackRange = 2;
    public int attackSpeed = 60;
    public string ability = "Whack";
    public int maxShield = 0;
    public int currentShield = 0;
    public bool marred = false;
    //public int priorityForAttacking

    public int moneyCost = 10;
    public int zealCost = 0;
    public int power;

    public MapUnit(string newName, Race newRace, string portrait) {
        name = newName;
        race = newRace;
        portraitName = portrait;
    }

    // Start is called before the first frame update
    void Start(){
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void SetHealth(int newHealth) {
        maxHealth = newHealth;
        currentHealth = maxHealth;
    }

    public void Refresh() {
        currentHealth = Mathf.Min(currentHealth + 10, maxHealth);
    }

    public MapUnit DeepCopy() {
        MapUnit copy = new MapUnit(name, race, portraitName);
        copy.SetHealth(maxHealth);
        copy.damage = damage;
        copy.attackRange = attackRange;
        copy.attackSpeed = attackSpeed;
        copy.moneyCost = moneyCost;
        copy.zealCost = zealCost;
        copy.power = power;
        copy.maxShield = maxShield;
        copy.currentShield = currentShield;
        copy.marred = marred;
        return copy;
    }
}
