using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour{

    public bool inMenu = false;
    public List<Cooldown> cooldowns;
    public bool inSimulation = false;
    GameObject attackArmy;
    GameObject defendArmy;
    GameObject battleNode;
    public GameObject attackArmyMenu;
    public GameObject defendArmyMenu;
    public List<MapUnit> units = new List<MapUnit>();
    public List<MapUnit> attackers = new List<MapUnit>();
    public List<MapUnit> defenders = new List<MapUnit>();
    public List<MapUnit> attackersFrontRow = new List<MapUnit>();
    public List<MapUnit> attackersBackRow = new List<MapUnit>();
    public List<MapUnit> defendersBuildings = new List<MapUnit>(); 
    public List<MapUnit> defendersFrontRow = new List<MapUnit>();
    public List<MapUnit> defendersBackRow = new List<MapUnit>();

    int timer = 0;
    bool retreating = false;

    public BattleType battleType;

    // Start is called before the first frame update
    void Start(){
        attackArmyMenu = Tools.GetChildNamed(gameObject, "Attacking Army Menu");
        defendArmyMenu = Tools.GetChildNamed(gameObject, "Defending Army Menu");

    }

    // Update is called once per frame
    void Update(){
        if (inSimulation) RunSimulation();
    }

    public void EnterMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(0, 0, -10));
        Player.menuOpen = 1;
    }
    public void ExitMenu() {
        GetComponent<Panner>().SetTarget(new Vector3(20, 0, -10));
        Player.menuOpen = 0;
    }

    void SetupArmies(GameObject attackingArmy, GameObject defendingArmy) {
        attackArmy = attackingArmy;
        defendArmy = defendingArmy;
        MapUnit[] AFrontRow = attackingArmy.GetComponent<Army>().frontRow;
        MapUnit[] ABackRow = attackingArmy.GetComponent<Army>().backRow;
        MapUnit[] DFrontRow = defendingArmy.GetComponent<Army>().frontRow;
        MapUnit[] DBackRow = defendingArmy.GetComponent<Army>().backRow;
        units.Clear();
        attackers.Clear();
        defenders.Clear();
        attackersFrontRow.Clear();
        attackersBackRow.Clear();
        defendersBuildings.Clear();
        defendersFrontRow.Clear();
        defendersBackRow.Clear();

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
        GameObject defendingNode = defendingArmy.GetComponent<Army>().currentNode;
        Temple temple = defendingNode.GetComponent<Node>().temple;
        Altar altar = defendingNode.GetComponent<Node>().altar;
        if (temple != null) {
            units.Add(temple.unit);
            defenders.Add(temple.unit);
            defendersBuildings.Add(temple.unit);
        }
        if (altar != null) {
            units.Add(altar.unit);
            defenders.Add(altar.unit);
            defendersBuildings.Add(altar.unit);
        }
    }

    public void SetupBattle(GameObject attackingArmy, GameObject defendingArmy, GameObject node) {
        SetupArmies(attackingArmy, defendingArmy);
        StartingCooldowns();
        battleNode = node;
    }



    void StartingCooldowns() {
        cooldowns = new List<Cooldown>();
        for (int i = 0; i < attackers.Count; i++) {
            int rand = Random.Range(0, 200);
            Cooldown newCooldown = new Cooldown(rand, attackers[i], "attacker");
            cooldowns.Add(newCooldown);
        }
        for (int i = 0; i < defenders.Count; i++) {
            int rand = Random.Range(0, 200);
            Cooldown newCooldown = new Cooldown(rand, defenders[i], "defender");
            cooldowns.Add(newCooldown);
        }

        cooldowns.Sort(Tools.SortByTime);

        for (int i = 0; i < cooldowns.Count; i++) {
            //print(cooldowns[i].timeToAct);
        }
    }

    public void InstantBattle() {
        //print("battle beginning");
        //print("attackers left: " + attackers.Count);
        //print("defenders left: " + defenders.Count);
        bool armyDefeated = false;
        if (defenders.Count <= 0) armyDefeated = true;
        while (!armyDefeated) {
            timer = cooldowns[0].timeToAct;
            // Run Next Attack
            armyDefeated = NextAttack(false);
        }
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendArmy);
        timer = 0;
    }
    
    bool NextAttack(bool retreating) {
        MapUnit attacker = cooldowns[0].unit;
        MapUnit target = GetRandomEnemy(cooldowns[0]);
        GameObject attackingUnitArmy, defendingUnitArmy;
        if (cooldowns[0].side == "attacker") {
            attackingUnitArmy = attackArmy;
            defendingUnitArmy = defendArmy;
        }
        else {
            attackingUnitArmy = defendArmy;
            defendingUnitArmy = attackArmy;
        }

        if (target != null) {
            if (target.currentShield > 0) target.currentShield--;
            else {
                target.currentHealth -= attacker.damage;
                // If the unit dealing damage is on the attacking side, the defending army triggers "Take Damage"
                defendingUnitArmy.GetComponent<Army>().owner.GetComponent<Player>().raceTraits.TakeDamage(target);
            }

            if (target.currentHealth <= 0) {
                defendingUnitArmy.GetComponent<Army>().owner.GetComponent<Player>().raceTraits.ArmyLostUnit(defendingUnitArmy);
                RemoveTargetFromCooldowns(target);
                RemoveUnitFromLists(target);
                RemoveUnitFromArmy(target);
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


    void RunSimulation() {
        timer++;
        if (cooldowns.Count != 0) {
            while (cooldowns.Count > 0 && timer == cooldowns[0].timeToAct) {
                // Run Next Attack
                inSimulation = !NextAttack(retreating);
                if (!inSimulation) {
                    cooldowns.RemoveAt(0);
                    cooldowns.Sort(Tools.SortByTime);
                }
                attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackArmy);
                defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendArmy);
                defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendArmy);
            }
        }
        else BattleOver();
    }

    public void StartSimulation() {
        inSimulation = true;
        timer = 0;
    }

    public void Retreat() {
        if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>()) attackArmy.GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
        if (inSimulation) {
            RemoveAttackCooldowns();
            retreating = true;
        }
        else {
            ExitMenu();
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
        //print("battle over");
        //print("attackers left: " + attackers.Count);
        //print("defenders left: " + defenders.Count);
        if (attackers.Count == 0) {
            attackArmy.GetComponent<Army>().Defeated();
        }
        else if (defenders.Count == 0) {
            if (defendArmy.GetComponent<Army>().race != Race.Independent){
                defendArmy.GetComponent<Army>().owner.GetComponent<Player>().RemoveNode(battleNode);
                defendArmy.GetComponent<Army>().Defeated();
            }
            attackArmy.GetComponent<Army>().owner.GetComponent<Player>().AddNode(battleNode);
            attackArmy.GetComponent<Army>().MoveToNode(battleNode); 
        }
        if (retreating) {
            ExitMenu();
        }
        retreating = false;
        inSimulation = false;
        if (attackArmy.GetComponent<Army>().owner.GetComponent<AI>()) attackArmy.GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
        print("Action Complete, readyToExecute");
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

    public void RemoveUnitFromArmy(MapUnit unit) {
        Army AScript = attackArmy.GetComponent<Army>();
        Army DScript = defendArmy.GetComponent<Army>();
        RemoveUnitFromArray(AScript.units, unit);
        RemoveUnitFromArray(AScript.backRow, unit);
        RemoveUnitFromArray(AScript.frontRow, unit);
        RemoveUnitFromArray(DScript.units, unit);
        RemoveUnitFromArray(DScript.backRow, unit);
        RemoveUnitFromArray(DScript.frontRow, unit);
    }

    public void RemoveUnitFromArray(MapUnit[] array, MapUnit unit) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == unit) {
                array[i] = null;
            }
        }
    }

    public bool IsBattleOver() {
        if (attackers.Count <= 0 || defenders.Count <= 0) return true;
        return false;
    }

}
