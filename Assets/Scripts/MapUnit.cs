using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit
{
    public string name = "Unit";
    public Race race = Race.Independent;
    public string portraitName = "zergling";

    public int maxHealth = 100;
    public int currentHealth = 80;
    public int damage = 10;
    public int attackRange = 2;
    public float attackSpeed = 1.0f;
    public string ability = "Whack";

    public int cost = 10;


    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
