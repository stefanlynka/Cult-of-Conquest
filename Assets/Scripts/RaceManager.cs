using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour{

    //RaceTraits traits = new RaceTraits();

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    public RaceTraits GetRaceTraits(Race race) {
        RaceTraits traits = new RaceTraits();
        switch (race) {
            case Race.Noumenon:
                MakeNoumenon(traits);
                break;
            case Race.Dukkha:
                MakeDukkha(traits);
                break;
            case Race.Paratrophs:
                MakeParatrophs(traits);
                break;
            case Race.Unmar:
                MakeUnmar(traits);
                break;
            case Race.Eidalons:
                MakeEidalons(traits);
                break;
            case Race.Carnot:
                MakeCarnot(traits);
                break;
            case Race.Independent:
                MakeIndependent(traits);
                break;
        }
        return traits;
    }

    void MakeNoumenon(RaceTraits traits) {
        traits.NewUnit = GiveShield;
        traits.TakeDamage = UnitBecomeMarred;
        traits.ArmyLostUnit = ArmyBecomeMarred;
    }
    void MakeDukkha(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }
    void MakeParatrophs(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }
    void MakeUnmar(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }
    void MakeEidalons(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }
    void MakeCarnot(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }
    void MakeIndependent(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
    }



    public void GiveShield(MapUnit unit) {
        unit.maxShield = 1;
        unit.currentShield = 1;
    }
    public void UnitBecomeMarred(MapUnit unit){
        if (unit.marred == false) {
            unit.marred = true;
            unit.damage = Mathf.RoundToInt(unit.damage * 0.8f);
        }
    }

    public void ArmyBecomeMarred(GameObject army) {
        if (!army.GetComponent<Army>().marred) {
            army.GetComponent<Army>().marred = true;
        }
        print("WE LOST ONE");
    }

    public void Empty(MapUnit unit) {}
    public void Empty() {}
    public void Empty(GameObject nothing) {}

    public int TestFunction(int number) {
        return number * 2;
    }
}

public class RaceTraits {
    public Race race;
    public delegate int DelegateTest(int number);
    public delegate void DelegateVoid();
    public delegate void DelegateGameObject(GameObject target);
    public delegate void DelegateUnit(MapUnit unit);
    public DelegateUnit NewUnit;
    public DelegateUnit KilledEnemy;
    public DelegateUnit TakeDamage;
    public DelegateGameObject ArmyLostUnit;
    public DelegateGameObject RetreatedAgainst; 
}