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
        traits.PrecombatAttacker = BoostAttacker;
        traits.PrecombatDefender = ShadowDefense;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = Empty;
        traits.KilledEnemy = Empty;
        traits.EnemyRetreated = InducedVictory;
        traits.BattleOver = Empty;
        traits.WonBattle = Empty;
        traits.StartTurn = ResetVision;
        traits.EndTurn = IncreaseFog;
    }
    void MakeDukkha(FactionTraits traits) {
        traits.NewUnit = Empty;
        traits.Precombat = Empty;
        traits.PrecombatAttacker = Empty;
        traits.PrecombatDefender = Empty;
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
        traits.PrecombatAttacker = StolenPrecombatAttack;
        traits.PrecombatDefender = StolenPrecombatDefense;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = DownToOneCheck;
        traits.KilledEnemy = StoreEnemy;
        traits.EnemyRetreated = Empty;
        traits.BattleOver = ReassembleEnemies;
        traits.WonBattle = Empty;
        traits.StartTurn = ResetBlueprints;
        traits.EndTurn = IncreaseFog;
    }
    void MakeUnmar(FactionTraits traits) { 
        traits.NewUnit = GiveShield;
        traits.Precombat = BoostMarred;
        traits.PrecombatAttacker = Empty;
        traits.PrecombatDefender = BoostDefense;
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
        traits.PrecombatAttacker = AgainstStrongest;
        traits.PrecombatDefender = Empty;
        traits.TakeDamage = Empty;
        traits.ArmyLostUnit = DownToOneCheck;
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
        traits.PrecombatAttacker = Empty;
        traits.PrecombatDefender = ProtectExplorers;
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
        traits.PrecombatAttacker = Empty;
        traits.PrecombatDefender = Empty;
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
    public void UnitBecomeMarred(GameObject army, MapUnit unit){
        print("mar check");
        if (unit.marred == false) {
            print("got marred");
            unit.marred = true;
            unit.marredCountdown = 2;
            unit.damageMod += army.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Doomed Power"].currentLevel * 0.15f;
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
            //If nextUnit.name.Contains("Altar")
            UnitPos position = army.GetComponent<Army>().GetOpenPosition();
            army.GetComponent<Army>().AddUnit(position.position, position.frontRow, nextUnit);
        }
        army.GetComponent<Army>().defeatedEnemies.Clear();
    }
    public void ResetBlueprints(GameObject player) {
        if (player.GetComponent<Player>().ritualBackup.Count > 0) {
            player.GetComponent<Player>().ritualBlueprints = Tools.DeepCopyRitualList(player.GetComponent<Player>().ritualBackup);
        }
        player.GetComponent<Player>().ritualBackup.Clear();
        GameObject.Find("Ritual Menu").GetComponent<RitualManager>().LoadPlayerRituals();
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
        float allyLostPower = army.GetComponent<Army>().precombatPower - army.GetComponent<Army>().GetOffensivePower();
        List<MapUnit> defeatedEnemies = army.GetComponent<Army>().defeatedEnemies;
        int enemyLostPower = 0;
        for (int i = 0; i < defeatedEnemies.Count; i++) {
            enemyLostPower += defeatedEnemies[i].power;
        }
        float minLostPower = Mathf.Min(allyLostPower,enemyLostPower);
        int rewardMoney = (int)minLostPower / 2;
        rewardMoney *= (int)(1 + (0.15f * army.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Reap Just Rewards"].currentLevel));
        army.GetComponent<Army>().owner.GetComponent<Player>().money += rewardMoney;
    }
    public void BalanceAdvantage(GameObject samataArmy, GameObject otherArmy) {
        if (samataArmy.GetComponent<Army>() && otherArmy.GetComponent<Army>()) {
            float vulnerability = 1.0f;
            float overPoweredRatio = (samataArmy.GetComponent<Army>().GetOffensivePower() / otherArmy.GetComponent<Army>().GetOffensivePower()) - 1;
            if (overPoweredRatio > 0) {
                vulnerability += overPoweredRatio * 0.75f;
            }
            for (int i = 0; i < samataArmy.GetComponent<Army>().units.Count; i++) {
                samataArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
            }
        }
    }
    public void PunishDisadvantage(GameObject noumenonArmy, GameObject otherArmy) {
        float vulnerability = 1.0f;
        float overPoweredRatio = (otherArmy.GetComponent<Army>().GetOffensivePower() / noumenonArmy.GetComponent<Army>().GetOffensivePower()) - 1;
        if (overPoweredRatio > 0) {
            vulnerability += overPoweredRatio * 0.75f;
        }
        for (int i = 0; i < noumenonArmy.GetComponent<Army>().units.Count; i++) {
            noumenonArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
        }
    }
    public void PunishSafety(GameObject carnotArmy, GameObject otherArmy) {
        float safetyRatio = carnotArmy.GetComponent<Army>().currentNode.GetComponent<Node>().GetSafety();
        float vulnerability = 1.0f + (safetyRatio / 2);
        for (int i = 0; i < carnotArmy.GetComponent<Army>().units.Count; i++) {
            carnotArmy.GetComponent<Army>().units[i].vulnerableMod = vulnerability;
        }
    }
    public void MarredCheck(GameObject player) {
        List<GameObject> armies = player.GetComponent<Player>().armies;
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            for (int j = 0; j < army.GetComponent<Army>().units.Count; j++) {
                MapUnit unit = army.GetComponent<Army>().units[j];
                if (unit.marred) {
                    unit.marredCountdown--;
                    if (unit.marredCountdown <= 0) army.GetComponent<Army>().RemoveUnit(unit);
                }
            }
        }
    }
    public void ResetVision(GameObject player) {
        List<GameObject> nodes = NodeManager.nodes;
        for (int i = 0; i < nodes.Count; i++) {
            nodes[i].GetComponent<Node>().concealment = 1;
        }
        TurnManager.currentPlayer.GetComponent<Player>().DisplayFog();
    }
    public void BoostAttacker(GameObject noumenonArmy, GameObject defendingNode) {
        if (noumenonArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Strike First")){
            Upgrade attackUpgrade = noumenonArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Strike First"];
            float damageMod = attackUpgrade.currentLevel * 0.2f;
            noumenonArmy.GetComponent<Army>().AddToDamageMod(damageMod);
        }
    }
    public void IncreaseFog(GameObject player) {
        if (player.GetComponent<Player>().upgrades.ContainsKey("Cover Your Tracks")) {
            List<GameObject> nodes = player.GetComponent<Player>().ownedNodes;
            for (int i = 0; i < nodes.Count; i++) {
                GameObject node = nodes[i];
                node.GetComponent<Node>().concealment = 1 + player.GetComponent<Player>().upgrades["Cover Your Tracks"].currentLevel;
            }
        }
    }
    public void AgainstStrongest(GameObject samataArmy, GameObject defendingArmy) {
        GameObject defendingNode = defendingArmy.GetComponent<Army>().currentNode;
        if (samataArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Against Tyranny")) {
            if (defendingNode.GetComponent<Node>().owner && defendingNode.GetComponent<Node>().owner.GetComponent<Player>().ownedNodes.Count >= Tools.StrongestFactionNodeCount()) {
                samataArmy.GetComponent<Army>().AddToDamageMod(0.1f * samataArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Against Tyranny"].currentLevel);
                print("against the strongest");
            }
        }
    }
    public void DownToOneCheck(GameObject army) {
        if (army.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Last One Standing")) {
            if (army.GetComponent<Army>().units.Count == 1) {
                MapUnit lastUnit = army.GetComponent<Army>().units[0];
                lastUnit.currentHealth = lastUnit.maxHealth + (int)(lastUnit.maxHealth * 0.3f * army.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Last One Standing"].currentLevel);
                lastUnit.currentDamage = lastUnit.currentDamage + (int)(lastUnit.currentDamage * 0.1f * army.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Last One Standing"].currentLevel);
            }
        }
    }
    public void BoostMarred(GameObject unmarArmy, GameObject otherArmy) {
        for(int i = 0; i< unmarArmy.GetComponent<Army>().units.Count;i++) {
            MapUnit unit = unmarArmy.GetComponent<Army>().units[i];
            if (unit.marred) unit.damageMod += unmarArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Doomed Power"].currentLevel * 0.15f;
        }
    }
    public void BoostDefense(GameObject unmarArmy, GameObject otherArmy) {
        if (unmarArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Fortification")) {
            for (int i = 0; i < unmarArmy.GetComponent<Army>().units.Count; i++) {
                MapUnit unit = unmarArmy.GetComponent<Army>().units[i];
                if (unit.name == "Altar" || unit.name == "Temple") {
                    unit.damageMod += unmarArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Fortification"].currentLevel * 0.2f;
                    unit.currentHealth += (int)(unmarArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Fortification"].currentLevel * 0.2f);
                }
            }
        }
    }
    public void ShadowDefense(GameObject noumenonArmy, GameObject otherArmy) {
        if (noumenonArmy.GetComponent<Army>().currentNode.GetComponent<Node>().concealment >= 3) {
            if (noumenonArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Hide In Shadow"))
                noumenonArmy.GetComponent<Army>().AddToDamageMod(0.1f * noumenonArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Hide In Shadow"].currentLevel);
        }
    }
    public void ProtectExplorers(GameObject carnotArmy, GameObject otherArmy) {
        Dictionary<string, Upgrade> upgrades = carnotArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades;
        if (upgrades.ContainsKey("Entropic Explorer")) {
            float exposure = carnotArmy.GetComponent<Army>().currentNode.GetComponent<Node>().GetExposure();
            if (upgrades.ContainsKey("Entropic Explorer") && exposure > 0.5f) {
                carnotArmy.GetComponent<Army>().AddToDamageMod(exposure * 0.2f * upgrades["Entropic Explorer"].currentLevel);
            }
        }
    }
    public void StolenPrecombatAttack(GameObject paratrophArmy, GameObject defendingNode) {
        if (paratrophArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Against Tyranny")) {
            if (defendingNode.GetComponent<Node>().owner.GetComponent<Player>().ownedNodes.Count >= Tools.StrongestFactionNodeCount()) {
                paratrophArmy.GetComponent<Army>().AddToDamageMod(0.1f * paratrophArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Against Tyranny"].currentLevel);
                print("against the strongest");
            }
        }
        if (paratrophArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Strike First")) {
            Upgrade attackUpgrade = paratrophArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Strike First"];
            float damageMod = attackUpgrade.currentLevel * 0.2f;
            paratrophArmy.GetComponent<Army>().AddToDamageMod(damageMod);
        }
    }
    public void StolenPrecombatDefense(GameObject paratrophArmy, GameObject otherArmy) {
        Dictionary<string, Upgrade> upgrades = paratrophArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades;
        if (upgrades.ContainsKey("Entropic Explorer")) {
            float exposure = paratrophArmy.GetComponent<Army>().currentNode.GetComponent<Node>().GetExposure();
            if (upgrades.ContainsKey("Entropic Explorer") && exposure > 0.5f) {
                paratrophArmy.GetComponent<Army>().AddToDamageMod(exposure * 0.2f * upgrades["Entropic Explorer"].currentLevel);
            }
        }
        if (upgrades.ContainsKey("Fortification")) {
            for (int i = 0; i < paratrophArmy.GetComponent<Army>().units.Count; i++) {
                MapUnit unit = paratrophArmy.GetComponent<Army>().units[i];
                if (unit.name == "Altar" || unit.name == "Temple") {
                    unit.damageMod += upgrades["Fortification"].currentLevel * 0.2f;
                    unit.currentHealth += (int)(upgrades["Fortification"].currentLevel * 0.2f);
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
    public DelegateObjectUnit TakeDamage;
    public DelegateObjectUnit KilledEnemy;
    public DelegateGameObject ArmyLostUnit;
    public DelegateGameObject EnemyRetreated;
    public DelegateGameObject WonBattle;
    public DelegateGameObject BattleOver;
    public DelegateGameObject StartTurn;
    public DelegateGameObject EndTurn;
    public DelegateGameObject2 Precombat;
    public DelegateGameObject2 PrecombatAttacker;
    public DelegateGameObject2 PrecombatDefender;
}