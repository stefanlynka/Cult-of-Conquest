using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour{

    //FactionTraits traits = new FactionTraits();

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    public FactionTraits GetFactionTraits(Faction faction) {
        FactionTraits traits = new FactionTraits();
        switch (faction) {
            case Faction.Noumenon:
                MakeNoumenon(traits);
                break;
            case Faction.Dukkha:
                MakeDukkha(traits);
                break;
            case Faction.Paratrophs:
                MakeParatrophs(traits);
                break;
            case Faction.Unmar:
                MakeUnmar(traits);
                break;
            case Faction.Samata:
                MakeSamata(traits);
                break;
            case Faction.Carnot:
                MakeCarnot(traits);
                break;
            case Faction.Independent:
                MakeIndependent(traits);
                break;
        }
        return traits;
    }

    void MakeNoumenon(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = PunishDisadvantage;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = InducedVictory;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
        traits.StartTurn = Empty;
        traits.EndTurn = Empty;
    }
    void MakeDukkha(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
        traits.StartTurn = Empty;
        traits.EndTurn = Empty;
    }
    void MakeParatrophs(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = StoreEnemy;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = ReassembleEnemies;
        traits.WonBattle = Empty;
        traits.StartTurn = Empty;
        traits.EndTurn = Empty;
    }
    void MakeUnmar(FactionTraits traits) { 
        traits.NewUnit = GiveShield;
        traits.Precombat = Empty;
        traits.TakeDamage = UnitBecomeMarred;
        traits.ArmyLostUnit = ArmyBecomeMarred;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = WinUnmarred;
        traits.StartTurn = MarredCheck;
        traits.EndTurn = Empty;
    }
    void MakeSamata(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = BalanceAdvantage;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = RewardsForFairness;
        traits.WonBattle = Empty;
        traits.StartTurn = Empty;
        traits.EndTurn = BalanceCheck;
    }
    void MakeCarnot(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
        traits.StartTurn = IsolationCheck;
        traits.EndTurn = Empty;
    }
    void MakeIndependent(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
        traits.StartTurn = Empty;
        traits.EndTurn = Empty;
    }



    public void GiveShield(MapUnit unit) {
        unit.maxShield = 1;
        unit.currentShield = 1;
    }
    public void UnitBecomeMarred(MapUnit unit){
        if (unit.marred == false) {
            unit.marred = true;
            unit.marredCountdown = 2;
            unit.maxDamage = Mathf.RoundToInt(unit.maxDamage * 0.8f);
            unit.currentDamage = Mathf.RoundToInt(unit.currentDamage * 0.8f);
        }
    }
    public void ArmyBecomeMarred(GameObject army) {
        if (!army.GetComponent<Army>().marredBattle) {
            army.GetComponent<Army>().marredBattle = true;
        }
    }
    public void WinUnmarred(GameObject army) {
        if (!army.GetComponent<Army>().marredBattle) {
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
    public void IsolationCheck(GameObject player) {
        List<GameObject> armies = player.GetComponent<Player>().armies;
        int numIsolated = 0;
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            if (army.GetComponent<Army>().Isolated()) numIsolated++;
        }
        player.GetComponent<Player>().zeal += numIsolated;
    }
    public void BalanceCheck(GameObject player) {
        List<int> nodeCounts = new List<int>();
        int totalNodes = 0;
        for(int i =0; i< TurnManager.players.Count; i++) {
            GameObject currentPlayer = TurnManager.players[i];
            int newNodeCount = currentPlayer.GetComponent<Player>().GetNodeCount();
            nodeCounts.Add(newNodeCount);
            totalNodes += newNodeCount;
        }
        int averageNodes = totalNodes / TurnManager.players.Count;
        int score = 0;
        for(int i =0; i < nodeCounts.Count; i++) {
            if (Mathf.Abs(averageNodes - nodeCounts[i]) < 1) score += 2;
            else if (Mathf.Abs(averageNodes - nodeCounts[i]) < 2) score += 1;
        }
        player.GetComponent<Player>().zeal += score;
    }
    public void RewardsForFairness(GameObject army) {
        int allyLostPower = army.GetComponent<Army>().precombatPower - army.GetComponent<Army>().GetPower();
        List<MapUnit> defeatedEnemies = army.GetComponent<Army>().defeatedEnemies;
        int enemyLostPower = 0;
        for (int i = 0; i < defeatedEnemies.Count; i++) {
            enemyLostPower += defeatedEnemies[i].power;
        }
        int minLostPower = Mathf.Min(allyLostPower,enemyLostPower);
        int rewardMoney = minLostPower / 2;
        army.GetComponent<Army>().owner.GetComponent<Player>().money += rewardMoney;
    }
    public void BalanceAdvantage(GameObject samataArmy, GameObject otherArmy) {
        float vulnerability = 1.0f;
        float overPoweredRatio = (samataArmy.GetComponent<Army>().GetPower() / otherArmy.GetComponent<Army>().GetPower()) - 1;
        if (overPoweredRatio > 0) {
            vulnerability += overPoweredRatio * 0.75f;
        }
        for (int i = 0; i < samataArmy.GetComponent<Army>().units.Length; i++) {
            if (samataArmy.GetComponent<Army>().units[i] != null) samataArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
        }
    }
    public void PunishDisadvantage(GameObject noumenonArmy, GameObject otherArmy) {
        float vulnerability = 1.0f;
        float overPoweredRatio = (otherArmy.GetComponent<Army>().GetPower() / noumenonArmy.GetComponent<Army>().GetPower()) - 1;
        if (overPoweredRatio > 0) {
            vulnerability += overPoweredRatio * 0.75f;
        }
        for (int i = 0; i < noumenonArmy.GetComponent<Army>().units.Length; i++) {
            if (noumenonArmy.GetComponent<Army>().units[i] != null) noumenonArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
        }
    }
    public void PunishSafety(GameObject carnotArmy, GameObject otherArmy) {
        float safetyRatio = carnotArmy.GetComponent<Army>().currentNode.GetComponent<Node>().GetSafety();
        float vulnerability = 1.0f + (safetyRatio / 2);
        for (int i = 0; i < carnotArmy.GetComponent<Army>().units.Length; i++) {
            if (carnotArmy.GetComponent<Army>().units[i] != null) carnotArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
        }
    }
    public void MarredCheck(GameObject player) {
        List<GameObject> armies = player.GetComponent<Player>().armies;
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            for (int j = 0; j < army.GetComponent<Army>().units.Length; j++) {
                MapUnit unit = army.GetComponent<Army>().units[j];
                if (unit != null) {
                    if (unit.marred) {
                        unit.marredCountdown--;
                        if (unit.marredCountdown <= 0) army.GetComponent<Army>().RemoveUnit(unit);
                    }
                }
            }
        }
    }


    public void Empty(MapUnit unit) {}
    public void Empty() {}
    public void Empty(GameObject nothing) {}
    public void Empty(GameObject nothing, MapUnit noone) { }
    public void Empty(GameObject nothing1, GameObject nothing2) { }

    public int TestFunction(int number) {
        return number * 2;
    }
}

public class FactionTraits {
    public Faction faction;
    public delegate int DelegateTest(int number);
    public delegate void DelegateVoid();
    public delegate void DelegateGameObject(GameObject target);
    public delegate void DelegateGameObject2(GameObject target1, GameObject target2);
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
    public DelegateGameObject StartTurn;
    public DelegateGameObject EndTurn;
    public DelegateGameObject2 Precombat;
}