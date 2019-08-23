using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit
{
    public string name;
    public Faction faction = Faction.Independent;
    public string portraitName = "zergling";

    public int maxHealth = 100;
    public int currentHealth;
    public int maxDamage = 10;
    public int currentDamage = 10;
    public int attackRange = 2;
    public int attackSpeed = 60;
    public string ability = "Whack";
    public float damageMod = 1.0f;
    public float vulnerableMod = 1.0f;

    public int maxShield = 0;
    public int currentShield = 0;
    public bool marred = false;
    public int marredCountdown = 0;
    public bool hidden = false;
    //public int priorityForAttacking

    public int moneyCost = 10;
    public int zealCost = 0;
    public int power;
    public int visiblePower;

    public MapUnit(string newName, Faction newFaction, string portrait) {
        name = newName;
        faction = newFaction;
        portraitName = portrait;
    }

    // Start is called before the first frame update
    void Start(){
        currentHealth = maxHealth;
        currentDamage = maxDamage;
        visiblePower = power;
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void SetHealth(int newHealth) {
        maxHealth = newHealth;
        currentHealth = maxHealth;
    }
    public void TrySetHealth(int newHealth) {
        currentHealth = Mathf.Min(maxHealth, newHealth);
    }

    public void Reset() {
        currentShield = maxShield;
        currentHealth = maxHealth;
        damageMod = 1.0f;
        vulnerableMod = 1.0f;
        currentDamage = maxDamage;
    }

    public void Refresh() {
        currentHealth = Mathf.Min(currentHealth + 10, maxHealth);
    }

    public MapUnit DeepCopy() {
        MapUnit copy = new MapUnit(name, faction, portraitName);
        //copy.SetHealth(maxHealth);
        copy.currentHealth = currentHealth;
        copy.maxHealth = maxHealth;
        copy.maxDamage = maxDamage;
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
