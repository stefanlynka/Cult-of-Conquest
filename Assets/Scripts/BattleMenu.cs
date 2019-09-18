using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour{

    public static bool retreatAllowed = true;

    public bool inMenu = false;
    public List<Cooldown> cooldowns;
    public bool inSimulation = false;
    GameObject attackArmy;
    GameObject defendArmy;
    public GameObject attackArmyMenu;
    public GameObject defendArmyMenu;
    public GameObject attackingPlayer, defendingPlayer, defendNode;
    public List<MapUnit> units = new List<MapUnit>();
    public List<MapUnit> attackers = new List<MapUnit>();
    public List<MapUnit> defenders = new List<MapUnit>();
    public List<MapUnit> attackersFrontRow = new List<MapUnit>();
    public List<MapUnit> attackersBackRow = new List<MapUnit>();
    public List<MapUnit> defendersBuildings = new List<MapUnit>(); 
    public List<MapUnit> defendersFrontRow = new List<MapUnit>();
    public List<MapUnit> defendersBackRow = new List<MapUnit>();
    GameObject buttonPanel, simButton, instantButton, retreatButton, backButton;

    int timer = 0;
    bool retreating = false;
    bool inBattleMenu = false;

    public BattleType battleType;

    // Start is called before the first frame update
    void Start(){
        attackArmyMenu = Tools.GetChildNamed(gameObject, "Attacking Army Menu");
        defendArmyMenu = Tools.GetChildNamed(gameObject, "Defending Army Menu");
        buttonPanel = Tools.GetChildNamed(gameObject, "Battle Button Panel");
        simButton = Tools.GetChildNamed(buttonPanel, "Sim Button");
        instantButton = Tools.GetChildNamed(buttonPanel, "Instant Button");
        retreatButton = Tools.GetChildNamed(buttonPanel, "Retreat Button");
        backButton = Tools.GetChildNamed(buttonPanel, "Back Button");
    }

    // Update is called once per frame
    void Update(){
        if (inSimulation) RunSimulation();
        if (inBattleMenu) CheckAIDecision();
    }

    void CheckAIDecision() {
        if (attackArmy && attackArmy.GetComponent<Army>().owner.GetComponent<AI>() && !DefenderIsHuman(defendNode)) {
            //retreatButton.SetActive(false);
            if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>().WantsToFight(attackArmy, defendNode) || !retreatAllowed) {
                InstantBattle();
            }
            else if (retreatAllowed) Retreat();
            ExitMenu();
        }
    }
    bool DefenderIsHuman(GameObject node) {
        if (node.GetComponent<Node>().owner && !node.GetComponent<Node>().owner.GetComponent<AI>() && node.GetComponent<Node>().faction != Faction.Independent) return true;
        return false;
    }

    public bool AIAttackingHuman() {
        if (attackArmy && attackArmy.GetComponent<Army>().owner.GetComponent<AI>() && DefenderIsHuman(defendNode)) return true;
        return false;
    }

    public bool AIWantsToFight() {
        if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>().WantsToFight(attackArmy, defendNode) || !retreatAllowed) return true;
        return false;
    }

    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -10));
        EnableButtons();
        SetBackButton(false);
        Player.menuOpen = 1;
        if (IsBattleOver()) BattleOver();
        inBattleMenu = true;
    }

    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(20, 0, -10));
        Player.menuOpen = 0;
        if (attackArmy && attackArmy.GetComponent<Army>().owner.GetComponent<AI>()) attackArmy.GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
        else if (attackingPlayer && attackingPlayer.GetComponent<AI>()) attackingPlayer.GetComponent<AI>().readyToExecute = true;
        Army.readyToMove = true;
        EnableButtons();
        inBattleMenu = false;
        attackArmyMenu.GetComponent<ArmyMenu>().CleanUnitSpaces();
        defendArmyMenu.GetComponent<ArmyMenu>().CleanUnitSpaces();
    }


    void DisableButtons() {
        simButton.SetActive(false);
        instantButton.SetActive(false);
        retreatButton.SetActive(false);
        SetBackButton(true);
    }
    void SetBackButton(bool active) {
        backButton.SetActive(active);
    }

    void EnableButtons() {
        simButton.SetActive(true);
        instantButton.SetActive(true);
        retreatButton.SetActive(true);
    }

    void ClearRows() {
        units.Clear();
        attackers.Clear();
        defenders.Clear();
        attackersFrontRow.Clear();
        attackersBackRow.Clear();
        defendersBuildings.Clear();
        defendersFrontRow.Clear();
        defendersBackRow.Clear();
        attackArmy = null;
        defendArmy = null;
        defendNode = null;
        attackingPlayer = null;
        defendingPlayer = null;
    }

    void SetupRows() {
        if (defendNode.GetComponent<Node>().occupant) {
            defendArmy = defendNode.GetComponent<Node>().occupant;
            MapUnit[] DFrontRow = defendArmy.GetComponent<Army>().frontRow;
            MapUnit[] DBackRow = defendArmy.GetComponent<Army>().backRow;
            for (int i = 0; i < DFrontRow.Length; i++) {
                if (DFrontRow[i] != null) {
                    units.Add(DFrontRow[i]);
                    defenders.Add(DFrontRow[i]);
                    defendersFrontRow.Add(DFrontRow[i]);
                }
            }
            for (int i = 0; i < DBackRow.Length; i++) {
                if (DBackRow[i] != null) {
                    units.Add(DBackRow[i]);
                    defenders.Add(DBackRow[i]);
                    defendersBackRow.Add(DBackRow[i]);
                }
            }
        }
        MapUnit[] AFrontRow = attackArmy.GetComponent<Army>().frontRow;
        MapUnit[] ABackRow = attackArmy.GetComponent<Army>().backRow;

        for (int i = 0; i < AFrontRow.Length; i++) {
            if (AFrontRow[i] != null) {
                units.Add(AFrontRow[i]);
                attackers.Add(AFrontRow[i]);
                attackersFrontRow.Add(AFrontRow[i]);
            }
        }
        for (int i = 0; i < ABackRow.Length; i++) {
            if (ABackRow[i] != null) {
                units.Add(ABackRow[i]);
                attackers.Add(ABackRow[i]);
                attackersBackRow.Add(ABackRow[i]);
            }
        }
        Temple temple = defendNode.GetComponent<Node>().temple;
        Altar altar = defendNode.GetComponent<Node>().altar;
        if (temple != null && temple.name == TempleName.Protection) {
            units.Add(temple.unit);
            defenders.Add(temple.unit);
            defendersBuildings.Add(temple.unit);
        }
        if (altar != null && altar.name == AltarName.Conflict) {
            units.Add(altar.unit);
            defenders.Add(altar.unit);
            defendersBuildings.Add(altar.unit);
        }
    }

    void SetupArmies(GameObject attackingArmy, GameObject defendingNode) {
        ClearRows();

        attackArmy = attackingArmy;
        defendNode = defendingNode;
        attackingPlayer = attackingArmy.GetComponent<Army>().owner;
        defendingPlayer = defendingNode.GetComponent<Node>().owner;

        SetupRows();

        attackArmy.GetComponent<Army>().PrebattleSetup();
        if (defendArmy != null) {
            defendArmy.GetComponent<Army>().PrebattleSetup();
            attackingPlayer.GetComponent<Player>().factionTraits.Precombat(attackArmy, defendArmy);
            defendingPlayer.GetComponent<Player>().factionTraits.Precombat(defendArmy, attackArmy);
        }

        else {
            attackingPlayer.GetComponent<Player>().factionTraits.Precombat(attackArmy, defendingNode);
            if (defendingPlayer) defendingPlayer.GetComponent<Player>().factionTraits.Precombat(defendingNode, attackArmy);
        }
        if (defendArmy) attackingPlayer.GetComponent<Player>().factionTraits.PrecombatAttacker(attackArmy, defendArmy);
    }

    public void SetupBattle(GameObject attackingArmy, GameObject defendingNode) {
        SetupArmies(attackingArmy, defendingNode);
        StartingCooldowns();
        defendNode = defendingNode;
    }

    void StartingCooldowns() {
        cooldowns = new List<Cooldown>();
        for (int i = 0; i < attackers.Count; i++) {
            int rand = Random.Range(0, 100);
            Cooldown newCooldown = new Cooldown(rand, attackers[i], "attacker");
            cooldowns.Add(newCooldown);
        }
        for (int i = 0; i < defenders.Count; i++) {
            int rand = Random.Range(0, 100);
            Cooldown newCooldown = new Cooldown(rand, defenders[i], "defender");
            cooldowns.Add(newCooldown);
        }

        cooldowns.Sort(Tools.SortByTime);

        for (int i = 0; i < cooldowns.Count; i++) {
            //print(cooldowns[i].timeToAct);
        }
    }

    public void StartSimulation() {
        inSimulation = true;
        timer = -10;
    }

    bool AIRetreatCheck() {
        if (attackingPlayer.GetComponent<AI>()) {
            if (attackArmy.GetComponent<Army>().units.Count <= 3) {
                if (attackArmy.GetComponent<Army>().GetOffensivePower() < defendNode.GetComponent<Node>().GetDefensivePower()) {
                    return true;
                }
            }
        }
        return false;
    }

    void RunSimulation() {
        timer++;
        if (!retreating && AIRetreatCheck()) Retreat();
        if (cooldowns.Count != 0) {
            while (cooldowns.Count > 0 && timer == cooldowns[0].timeToAct && !IsBattleOver()) {
                // Run Next Attack
                inSimulation = !NextAttack(retreating);
                if (!inSimulation) {
                    cooldowns.RemoveAt(0);
                    cooldowns.Sort(Tools.SortByTime);
                }
                attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackArmy);
                if (defendArmy) defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendArmy);
                defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendNode);
            }
        }
        else BattleOver();
    }

    public void InstantBattle() {
        //print("attackers left: " + attackers.Count);
        //print("defenders left: " + defenders.Count);
        bool armyDefeated = false;
        if (defenders.Count <= 0) armyDefeated = true;
        while (cooldowns.Count > 0) {
            timer = cooldowns[0].timeToAct;
            if (!retreating && AIRetreatCheck()) {
                retreating = true;
                RemoveAttackCooldowns();
                if (defendArmy) defendingPlayer.GetComponent<Player>().factionTraits.EnemyRetreated(defendArmy);
                attackArmy.GetComponent<Army>().OrderToEnterNodeNow(attackArmy.GetComponent<Army>().currentNode);
            }
            // Run Next Attack
            armyDefeated = NextAttack(retreating);
            if (armyDefeated) cooldowns.Clear();
        }
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackArmy);
        if (defendArmy) defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendNode);
        timer = 0;
        if (retreating) BattleOver();
    }
    bool NextAttack(bool retreating) {
        MapUnit attacker = cooldowns[0].unit;
        MapUnit target = GetRandomEnemy(cooldowns[0]);
        GameObject attackingUnitArmy;
        GameObject defendingUnitArmy = null;
        if (cooldowns[0].side == "attacker") {
            attackingUnitArmy = attackArmy;
            if (defendArmy) defendingUnitArmy = defendArmy;
        }
        else {
            attackingUnitArmy = defendArmy;
            if (defendArmy) defendingUnitArmy = attackArmy;
        }

        if (target != null && attacker != null) {
            int damage = (int)(attacker.currentDamage * attacker.damageMod * target.vulnerableMod);
            if (attacker.fake) damage = 0;
            if (target.fake) damage *= 2;
            if (target.currentShield > 0 && damage > 0) target.currentShield--;
            else {
                target.currentHealth -= damage;
                //print("unit space "+attacker);
                Tools.CreatePopup(attacker.unitSpace, "Attacks for " + damage, 40, Color.red, 90, 0.005f);
                // If the unit dealing damage is on the attacking side, the defending army triggers "Take Damage"
                if (defendingUnitArmy && defendingUnitArmy.GetComponent<Army>().owner) defendingUnitArmy.GetComponent<Army>().owner.GetComponent<Player>().factionTraits.TakeDamage(defendingUnitArmy, target);
            }

            if (target.currentHealth <= 0) {
                if (defendingUnitArmy && defendingUnitArmy.GetComponent<Army>().owner) defendingUnitArmy.GetComponent<Army>().owner.GetComponent<Player>().factionTraits.ArmyLostUnit(defendingUnitArmy);
                attackingPlayer.GetComponent<Player>().factionTraits.KilledEnemy(attackingUnitArmy, target);
                RemoveTargetFromCooldowns(target);
                RemoveUnitFromLists(target);
                if (defendArmy) defendingUnitArmy.GetComponent<Army>().RemoveUnit(target);
            }
        }
        if (attackers.Count == 0 || defenders.Count == 0) {
            BattleOver();
            return true;
        }
        if (!retreating) {
            Cooldown newCooldown = new Cooldown(attacker.attackSpeed + timer, attacker, cooldowns[0].side);
            cooldowns.Add(newCooldown);
        }
        cooldowns.RemoveAt(0);
        cooldowns.Sort(Tools.SortByTime);
        return false;
    }

    public void Retreat() {
        if (retreatAllowed) {
            //if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>()) attackArmy.GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
            if (defendArmy) defendingPlayer.GetComponent<Player>().factionTraits.EnemyRetreated(defendArmy);
            attackArmy.GetComponent<Army>().OrderToEnterNodeNow(attackArmy.GetComponent<Army>().currentNode);
            //attackArmy.GetComponent<MoveAnimator>().SetTarget(attackArmy.GetComponent<Army>().currentNode.transform.position, false);
            if (inSimulation) {
                RemoveAttackCooldowns();
                retreating = true;
            }
            else {
                if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>() && defendArmy) {
                    attackArmy.GetComponent<Army>().owner.GetComponent<AI>().RememberArmy(defendArmy);
                }
                ExitMenu();
            }
        }
    }

    void RemoveAttackCooldowns() {
        for (int i =0; i < cooldowns.Count; i++) {
            if (cooldowns[i].side == "attacker") {
                cooldowns.RemoveAt(i);
                i--;
            }
        }
    }

    public void BattleOver() {
        if (defendArmy) {
            defendArmy.GetComponent<Army>().ResetArmy();
            if (defendingPlayer) defendingPlayer.GetComponent<Player>().factionTraits.BattleOver(defendArmy);
            if (attackers.Count == 0) defendingPlayer.GetComponent<Player>().factionTraits.WonBattle(defendArmy);
        }
        attackArmy.GetComponent<Army>().ResetArmy();
        attackingPlayer.GetComponent<Player>().factionTraits.BattleOver(attackArmy);
        RefreshBuildings();
        //GameObject winningArmy = defendArmy;

        if (defenders.Count == 0) attackingPlayer.GetComponent<Player>().factionTraits.WonBattle(attackArmy);
        if (attackers.Count == 0) {
            attackArmy.GetComponent<Army>().Defeated();
            Army.readyToMove = true;
        }
        else if (defenders.Count == 0) {
            AttackerWins();
        }
        if (retreating) {
            if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>() && defendArmy) {
                attackArmy.GetComponent<Army>().owner.GetComponent<AI>().RememberArmy(defendArmy);
            }
            ExitMenu();
        }
        retreating = false;
        inSimulation = false;
        //if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>()) attackArmy.GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
        //print("Action Complete, readyToExecute");
        retreatAllowed = true;
        //if (attackArmy.GetComponent<Army>().owner != Player.human) ExitMenu();
        TurnManager.currentPlayer.GetComponent<Player>().DisplayFog();
        DisableButtons();
        SetBackButton(true);
    }

    public void AttackerWins() {
        AttackerClaimsEffigy();
        if (defendingPlayer) defendingPlayer.GetComponent<Player>().RemoveNode(defendNode);
        if (defendArmy && defendArmy.GetComponent<Army>().faction != Faction.Independent) defendArmy.GetComponent<Army>().Defeated();

        attackingPlayer.GetComponent<Player>().AddNode(defendNode);
        attackArmy.GetComponent<Army>().MoveToNode(defendNode);
        if (!AttackerCanAssimilate()) {
            defendNode.GetComponent<Node>().temple = null;
            defendNode.GetComponent<Node>().altar = null;
        }
    }

    public void AttackerClaimsEffigy() {
        if (defendArmy) {
            if (defendArmy.GetComponent<Army>().effigy != null) {
                Effigy displacedEffigy = defendArmy.GetComponent<Army>().effigy;
                if (attackArmy.GetComponent<Army>().effigy == null) attackArmy.GetComponent<Army>().effigy = displacedEffigy;
                else if (defendNode.GetComponent<Node>().effigy != null) defendNode.GetComponent<Node>().effigy = displacedEffigy;
                else {
                    while (displacedEffigy != null) {
                        List<GameObject> playerNodes = attackingPlayer.GetComponent<Player>().ownedNodes;
                        GameObject randomNode = playerNodes[Random.Range(0, playerNodes.Count)];
                        if (randomNode.GetComponent<Node>().effigy == null) {
                            randomNode.GetComponent<Node>().effigy = displacedEffigy;
                            displacedEffigy = null;
                        }
                    }
                }
            }
        }
    }
    public void RefreshBuildings() {
        if (defendNode.GetComponent<Node>().temple != null) defendNode.GetComponent<Node>().temple.ResetUnit();
        if (defendNode.GetComponent<Node>().altar != null) defendNode.GetComponent<Node>().altar.ResetUnit();
    }
    public bool AttackerCanAssimilate() {
        if (attackingPlayer.GetComponent<Player>().upgrades.ContainsKey("Assimilate") && attackingPlayer.GetComponent<Player>().upgrades["Assimilate"].currentLevel >= 1) return true;
        return false;
    }

    public MapUnit GetRandomEnemy(Cooldown cooldown) {
        MapUnit target;
        if (cooldown.side == "attacker") {
            if (defendersBuildings.Count > 0) target = defendersBuildings[Random.Range(0, defendersBuildings.Count)];
            else if (defendersFrontRow.Count > 0) target = defendersFrontRow[Random.Range(0, defendersFrontRow.Count)];
            else if (defendersBackRow.Count > 0) target = defendersBackRow[Random.Range(0, defendersBackRow.Count)];
            else target = null;
        }
        else {
            if (attackersFrontRow.Count > 0) target = attackersFrontRow[Random.Range(0, attackersFrontRow.Count)];
            else if (attackersBackRow.Count > 0) target = attackersBackRow[Random.Range(0, attackersBackRow.Count)];
            else target = null;
        }

        return target;
    }

    public void RemoveTargetFromCooldowns(MapUnit target) {
        for (int i = 0; i < cooldowns.Count; i++) {
            if (cooldowns[i].unit == target) {
                cooldowns.RemoveAt(i);
                i--;
            }
        }
    }

    public void RemoveUnitFromLists(MapUnit unit) {
        units.Remove(unit);
        attackers.Remove(unit);
        defenders.Remove(unit);
        attackersBackRow.Remove(unit);
        attackersFrontRow.Remove(unit);
        defendersBackRow.Remove(unit);
        defendersFrontRow.Remove(unit);
        defendersBuildings.Remove(unit);
    }

    public bool IsBattleOver() {
        if (attackers.Count <= 0 || defenders.Count <= 0) return true;
        return false;
    }

}
