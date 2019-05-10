using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit
{
    public string name = "Unit";
    public Race race = Race.Independent;
    public string portraitName = "zergling";

    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public int attackRange = 2;
    public int attackSpeed = 60;
    public string ability = "Whack";

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
}
