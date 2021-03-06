﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit
{
    public string name;
    public Faction faction = Faction.Independent;
    public string portraitName = "peon";
    public GameObject unitSpace;

    public int maxHealth;
    public int currentHealth;
    public int maxDamage;
    public int currentDamage;
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
    int defaultDamage = 10;
    int defaultHealth = 100;
    int defaultPower = 10;
    //public int priorityForAttacking

    public int moneyCost = 10;
    public int zealCost = 0;
    public int power;
    public int visiblePower;
    public bool fake = false;

    public MapUnit(string newName, Faction newFaction, string portrait) {
        name = newName;
        faction = newFaction;
        portraitName = portrait;
        fake = false;
        power = defaultPower;
        visiblePower = defaultPower;
        SetupStats();
    }

    // Start is called before the first frame update
    void Start(){
        //SetupStats();
    }


    void SetupStats() {
        maxHealth = defaultHealth;
        currentHealth = defaultHealth;
        maxDamage = defaultDamage;
        currentDamage = defaultDamage;
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
    public void SetDamage(int newDamage) {
        maxDamage = newDamage;
        currentDamage = newDamage;
    }

    public void Reset() {
        currentShield = maxShield;
        //currentHealth = maxHealth;
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
        copy.maxHealth = maxHealth;
        copy.currentHealth = currentHealth;
        copy.maxDamage = maxDamage;
        copy.currentDamage = currentDamage;
        copy.attackRange = attackRange;
        copy.attackSpeed = attackSpeed;
        copy.moneyCost = moneyCost;
        copy.zealCost = zealCost;
        copy.power = power;
        copy.maxShield = maxShield;
        copy.currentShield = currentShield;
        copy.marred = marred;
        copy.fake = fake;
        return copy;
    }

    public float GetPower() {
        return (currentHealth + currentShield*10) * currentDamage / attackSpeed;
    }
    public float GetDPS() {
        return (float) maxDamage / attackSpeed;
    }
    public float GetHealth() {
        return (currentHealth + maxShield * 10);
    }
}
