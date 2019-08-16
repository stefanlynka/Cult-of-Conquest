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
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = InducedVictory;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
    }
    void MakeDukkha(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
    }
    void MakeParatrophs(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = StoreEnemy;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = ReassembleEnemies;
        traits.WonBattle = Empty;
    }
    void MakeUnmar(RaceTraits traits) {

        traits.NewUnit = GiveShield;
        traits.TakeDamage = UnitBecomeMarred;
        traits.ArmyLostUnit = ArmyBecomeMarred;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = WinUnmarred;
    }
    void MakeEidalons(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
    }
    void MakeCarnot(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
    }
    void MakeIndependent(RaceTraits traits) {
        traits.NewUnit = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
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
        if (!army.GetComponent<Army>().marredBattle) {
            army.GetComponent<Army>().marredBattle = true;
        }
        print("WE LOST ONE");
    }
    public void WinUnmarred(GameObject army) {
        if (!army.GetComponent<Army>().marredBattle) {
            print("Unmarred Victory!");
            army.GetComponent<Army>().owner.GetComponent<Player>().zeal++;
        }
        else army.GetComponent<Army>().marredBattle = false;
    }

    public void StoreEnemy(GameObject army, MapUnit unit) {
        MapUnit newUnit = unit.DeepCopy();
        newUnit.currentHealth = newUnit.maxHealth/4;
        newUnit.currentShield = newUnit.maxShield;
        army.GetComponent<Army>().defeatedEnemies.Add(newUnit);
    }
    public void ReassembleEnemies(GameObject army) {
        List<MapUnit> deadUnits = army.GetComponent<Army>().defeatedEnemies;
        deadUnits.Sort(Tools.SortByPower);
        while (army.GetComponent<Army>().HasOpenPosition() && deadUnits.Count>0) {
            MapUnit nextUnit = deadUnits[deadUnits.Count - 1];
            deadUnits.Remove(nextUnit);
            UnitPos position = army.GetComponent<Army>().GetOpenPosition();
            army.GetComponent<Army>().AddUnit(position.position, position.frontRow, nextUnit);
        }
        army.GetComponent<Army>().defeatedEnemies.Clear();
    }
    public void InducedVictory(GameObject army) {
        army.GetComponent<Army>().owner.GetComponent<Player>().zeal++;
    }

    public void Empty(MapUnit unit) {}
    public void Empty() {}
    public void Empty(GameObject nothing) {}
    public void Empty(GameObject nothing, MapUnit noone) { }

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
    public delegate void DelegateObjectUnit(GameObject target, MapUnit unit);
    public DelegateUnit NewUnit;
    //public DelegateUnit KilledEnemy;
    public DelegateUnit TakeDamage;
    public DelegateObjectUnit KilledEnemy;
    public DelegateGameObject ArmyLostUnit;
    public DelegateGameObject EnemyRetreated;
    public DelegateGameObject WonBattle;
    public DelegateGameObject BattleOver;
}