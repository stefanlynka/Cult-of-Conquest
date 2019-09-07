using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<Intent> intentQueue = new List<Intent>();
    public Dictionary<GameObject, ArmyInfo> enemyArmyInfo = new Dictionary<GameObject, ArmyInfo>();
    GameObject unitShop;

    Faction faction;
    public TurnPhase turnPhase = TurnPhase.DefendAgainstThreats;
    public bool readyToExecute = true;
    bool moreToDoInPhase = false;
    int unallocatedMoney = 0;


    // Start is called before the first frame update
    void Start() {
        CollectInformation();
    }

    void CollectInformation() {
        armies = GetComponent<Player>().armies;
        ownedNodes = GetComponent<Player>().ownedNodes;
        faction = GetComponent<Player>().faction;
    }

    // Update is called once per frame

    void Update() {
        print("Ready to Execute = :" + readyToExecute);
        print("Armies are ready: " + Army.readyToMove);
        if (TurnManager.currentPlayer == gameObject) {
            if (readyToExecute && Army.readyToMove && Army.movesToDo.Count == 0) {
                if (turnPhase == TurnPhase.DefendAgainstThreats) {
                    print("defend phase");
                    Defend();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Scouting;
                }
                else if (turnPhase == TurnPhase.Scouting) {
                    Scout();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Allocating;
                }
                else if (turnPhase == TurnPhase.Allocating) {
                    print("spend phase start");
                    AllocateMoney();
                    print("spend phase over");
                    //BuyUnits();
                    turnPhase = TurnPhase.Attacks;
                }
                else if (turnPhase == TurnPhase.Attacks) {
                    //print("attack phase");
                    Attack();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Done;
                }
                else if (turnPhase == TurnPhase.Done) {
                    print("Turn Over");
                }

            }
        }
    }




    void Defend() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (MoveToThreatenedNode(currentArmy)) {
                //print("moving, not ready to execute");
                moreToDoInPhase = true;
                //print("Move performed, waiting");
                return;
            }
        }
    }
    bool MoveToThreatenedNode(GameObject army) {

        //print("checking possible threats");
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        Tools.DeepCopyGameObjectList(ownedNodes);
        List<GameObject> nodesByThreat = Tools.DeepCopyGameObjectList(ownedNodes);
        nodesByThreat.Sort(Tools.SortByThreat);

        int index = 0;
        while (index < nodesByThreat.Count) {
            GameObject node = nodesByThreat[index];
            if (node.GetComponent<Node>().occupant != null) {
                nodesByThreat.Remove(node);
                index--;
            }
            if (nodesByThreat.Contains(node) && node.GetComponent<Node>().GetThreatToNode() <= 0) {
                nodesByThreat.Remove(node);
                index--;
            }
            index++;
        }

        for (int i = 0; i < nodesByThreat.Count; i++) {
            print("nodesByThreat: " + nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
        }

        GameObject armyNode = army.GetComponent<Army>().currentNode;
        List<GameObject> nodesInRange = new List<GameObject>();
        nodesInRange = GetNodesInRange(nodesInRange, armyNode, army.GetComponent<Army>().movesLeft);

        GameObject reachableThreatenedNode = null;
        int j = nodesByThreat.Count - 1;
        // Search nodes by threat descending, choose a reachable node
        while (reachableThreatenedNode == null && j >= 0) {
            GameObject node = nodesByThreat[j];
            if (nodesInRange.Contains(node)) {
                reachableThreatenedNode = node;
            }
            j--;
        }

        if (reachableThreatenedNode != null && reachableThreatenedNode != armyNode && MoreThreatenedThan(reachableThreatenedNode, armyNode)) {
            print("found someone worth protecting :')");
            readyToExecute = false;
            GetComponent<Player>().attackNode(army, reachableThreatenedNode);
            return true;
        }

        return false;
    }
    List<GameObject> GetNodesInRange(List<GameObject> nodes, GameObject node, int range) {
        if (!nodes.Contains(node)) nodes.Add(node);
        if (range > 0) {
            List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
            for (int i = 0; i < neighbours.Count; i++) {
                GameObject neighbour = neighbours[i];
                if (neighbour.GetComponent<Node>().faction == faction && !neighbour.GetComponent<Node>().occupied) GetNodesInRange(nodes, neighbour, range - 1);
            }
        }
        return nodes;
    }
    bool MoreThreatenedThan(GameObject node1, GameObject node2) {
        //print("Other threat: " + node1.GetComponent<Node>().GetThreatToNode());
        //print("Threat to me: " + node2.GetComponent<Node>().GetThreatIgnoringCurrentArmy());
        if (node1.GetComponent<Node>().GetThreatToNode() > node2.GetComponent<Node>().GetThreatIgnoringCurrentArmy()) return true;
        return false;
    }




    void Scout() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (currentArmy.GetComponent<Army>().movesLeft > 0 && NearbyUnscoutedArmy(currentArmy)) {
                //print("Attack performed, waiting");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
    }
    bool NearbyUnscoutedArmy(GameObject army) {
        GameObject node = army.GetComponent<Army>().currentNode;
        for (int i = 0; i < node.GetComponent<Node>().neighbours.Count; i++) {
            GameObject neighbourNode = node.GetComponent<Node>().neighbours[i];
            GameObject neighbouringArmy = neighbourNode.GetComponent<Node>().occupant;
            if (neighbouringArmy != null && neighbouringArmy.GetComponent<Army>().faction != faction && neighbouringArmy.GetComponent<Army>().faction != Faction.Independent) {
                if (!enemyArmyInfo.ContainsKey(neighbouringArmy) || (enemyArmyInfo.ContainsKey(neighbouringArmy) && enemyArmyInfo[neighbouringArmy].timeSinceScout >= 2)) {
                    print("Go Scout: " + neighbourNode.name);
                    readyToExecute = false;
                    RememberArmy(neighbouringArmy);
                    GetComponent<Player>().attackNode(army, neighbourNode);
                    return true;
                }
            }
        }
        return false;
    }
    public void RememberArmy(GameObject enemyArmy) {
        ArmyInfo newInfo = new ArmyInfo(0, enemyArmy.GetComponent<Army>().GetOffensivePower());
        if (enemyArmyInfo.ContainsKey(enemyArmy)) {
            enemyArmyInfo[enemyArmy] = newInfo;
        }
        else {
            enemyArmyInfo.Add(enemyArmy, newInfo);
        }
    }


    void AllocateMoney() {
        int totalMoneyNeeded = 0;
        Dictionary<GameObject, int> armyRequirements = new Dictionary<GameObject, int>();
        foreach (GameObject army in armies) {
            int moneyNeeded = GetMinMoneyNeeded(army);
            totalMoneyNeeded += moneyNeeded;
            armyRequirements.Add(army, moneyNeeded);
            army.GetComponent<Army>().allocatedMoney = moneyNeeded;
            //print("money needed: " + moneyNeeded);
        }
        //if (totalMoneyNeeded > unallocatedMoney) print("not enough money to allocate");
        //else print("Have enough money to allocate");
        //print("Total Money Needed: " + totalMoneyNeeded);
        int surplusMoney = GetComponent<Player>().money - totalMoneyNeeded;
        print("Prophet Util: " + GetProphetUtility());
        print("Army Util: " + GetArmyUtility());
        if (surplusMoney >= 0) {
            int investInProphets = 0;
            int investInArmies = 0;
            int investInReplenishing = 0;
            int investInBuildings = 0;

            //float prophetUtility = GetProphetUtility();
            //float armyUtility = GetArmyUtility() * 2;
            float replenishingUtility = 0;
            float buildingUtility = 0;
        }
    }
    int GetMinMoneyNeeded(GameObject army) {
        int moneyNeeded = 0;
        GameObject threat = GetGreatestThreat(army);
        if (threat != null && army.GetComponent<Army>().GetDefensivePower() < enemyArmyInfo[threat].expectedPower) {
            float threatLevel = enemyArmyInfo[threat].expectedPower;
            MapUnit[] frontRowDefault = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().frontRow);
            MapUnit[] backRowDefault = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().backRow);
            Temple templeDefault = null;
            if (army.GetComponent<Army>().currentNode.GetComponent<Node>().temple != null) templeDefault = army.GetComponent<Army>().currentNode.GetComponent<Node>().temple.DeepCopy();
            Altar altarDefault = null;
            if (army.GetComponent<Army>().currentNode.GetComponent<Node>().altar != null) altarDefault = army.GetComponent<Army>().currentNode.GetComponent<Node>().altar.DeepCopy();
            List<MapUnit> unitsNeeded = new List<MapUnit>();

            unitsNeeded = UnitsNeededByCopying(frontRowDefault, backRowDefault, templeDefault, altarDefault, threat);
            if (unitsNeeded.Count > 0) {
                print("Building " + unitsNeeded.Count + " units will be enough");
                for (int i = 0; i < unitsNeeded.Count; i++) {
                    moneyNeeded += unitsNeeded[i].moneyCost;
                    print("this unit cost: " + unitsNeeded[i].moneyCost);
                }
                army.GetComponent<Army>().unitsToAdd = unitsNeeded;
                return moneyNeeded;
            }

            unitsNeeded = UnitsNeededByCopyingMax(frontRowDefault, backRowDefault, templeDefault, altarDefault, threat);
            if (unitsNeeded.Count > 0) {
                print("Building the biggest unit will be enough");
                for (int i = 0; i < unitsNeeded.Count; i++) {
                    moneyNeeded += unitsNeeded[i].moneyCost;
                }
                army.GetComponent<Army>().unitsToAdd = unitsNeeded;
                return moneyNeeded;
            }

        }
        else print("Don't need money");
        army.GetComponent<Army>().unitsToAdd.Clear();
        return 0;
    }
    List<MapUnit> UnitsNeededByCopying(MapUnit[] FRDefault, MapUnit[] BRDefault, Temple templeDefault, Altar altarDefault, GameObject enemyArmy) {
        MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(FRDefault);
        MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(BRDefault);
        Temple templeSimulated = null;
        if (templeDefault != null) templeSimulated = templeDefault.DeepCopy();
        Altar altarSimulated = null;
        if (altarDefault != null) altarSimulated = altarDefault.DeepCopy();
        List<MapUnit> enemyUnits = enemyArmy.GetComponent<Army>().units;
        List<MapUnit> unitsNeeded = new List<MapUnit>();
        float threatLevel = enemyArmyInfo[enemyArmy].expectedPower;

        enemyUnits.Sort(Tools.SortByPower);
        int index = 1;
        int numOpenSpots = Tools.GetOpenSpotCount(frontRowSimulated, backRowSimulated);
        while (index <= enemyUnits.Count && index <= numOpenSpots && GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) < threatLevel) {
            MapUnit simulatedUnit = GetComponent<Player>().unitBlueprints[Tools.UnitToIndex(enemyUnits[enemyUnits.Count-index])].DeepCopy();
            Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
            unitsNeeded.Add(simulatedUnit);
            index++;
        }
        if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) return unitsNeeded;

        if (templeSimulated == null) {
            templeSimulated = GetComponent<Player>().templeBlueprints[TempleName.Protection].DeepCopy();
            MapUnit templeStandIn = new MapUnit("temple", Faction.None, "");
            templeStandIn.moneyCost = GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            unitsNeeded.Add(templeStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }
        if (altarSimulated == null) {
            altarSimulated = GetComponent<Player>().altarBlueprints[AltarName.Conflict].DeepCopy();
            MapUnit altarStandIn = new MapUnit("altar", Faction.None, "");
            altarStandIn.moneyCost = GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
            unitsNeeded.Add(altarStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }

        return new List<MapUnit>();
    }
    List<MapUnit> UnitsNeededByCopyingMax(MapUnit[] FRDefault, MapUnit[] BRDefault, Temple templeDefault, Altar altarDefault, GameObject enemyArmy) {
        MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(FRDefault);
        MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(BRDefault);
        Temple templeSimulated = null;
        if (templeDefault != null) templeSimulated = templeDefault.DeepCopy();
        Altar altarSimulated = null;
        if (altarDefault != null) altarSimulated = altarDefault.DeepCopy();
        List<MapUnit> enemyUnits = enemyArmy.GetComponent<Army>().units;
        List<MapUnit> unitsNeeded = new List<MapUnit>();
        float threatLevel = enemyArmyInfo[enemyArmy].expectedPower;

        enemyUnits.Sort(Tools.SortByPower);
        int biggestEnemyUnitIndex = Tools.UnitToIndex(enemyUnits[enemyUnits.Count - 1]);
        int index = 0;
        int numOpenSpots = Tools.GetOpenSpotCount(frontRowSimulated, backRowSimulated);
        while (index < numOpenSpots && GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) < threatLevel) {
            MapUnit simulatedUnit = GetComponent<Player>().unitBlueprints[biggestEnemyUnitIndex].DeepCopy();
            Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
            unitsNeeded.Add(simulatedUnit);
            index++;
        }
        if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
            return unitsNeeded;
        }

        if (templeSimulated == null) {
            templeSimulated = GetComponent<Player>().templeBlueprints[TempleName.Protection].DeepCopy();
            MapUnit templeStandIn = new MapUnit("temple", Faction.None, "");
            templeStandIn.moneyCost = GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            unitsNeeded.Add(templeStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }
        if (altarSimulated == null) {
            altarSimulated = GetComponent<Player>().altarBlueprints[AltarName.Conflict].DeepCopy();
            MapUnit altarStandIn = new MapUnit("altar", Faction.None, "");
            altarStandIn.moneyCost = GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
            unitsNeeded.Add(altarStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }

        return new List<MapUnit>();
    }
    float GetProphetUtility() {
        float localNodesToConquer = 9;
        float expandingUtility = localNodesToConquer / ownedNodes.Count;
        print("expanding Util: " + expandingUtility);

        float protectBordersUtility = (GetFrontierNodeCount() * 5) / (2 * armies.Count);
        print("protectborder Util: " + protectBordersUtility);
        return Mathf.Max(expandingUtility, protectBordersUtility);
    }
    float GetArmyUtility() {
        float maxUtility = 0;
        GameObject armyToImprove = null;
        int unitIndex = 0;

        foreach (GameObject army in armies) {
            if (army.GetComponent<Army>().allocatedMoney == 0) {
                float currentPower = army.GetComponent<Army>().GetOffensivePower();
                for (int i = 0; i < 4; i++) {
                    if (i == 2) i = 3;
                    print("Index: " + i);
                    print("Current Power: " + currentPower);
                    MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().frontRow);
                    MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().backRow);
                    MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                    if (army.GetComponent<Army>().HasOpenPosition()) Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, newUnit);
                    else Tools.ReplaceWeakestUnitInRows(frontRowSimulated, backRowSimulated, newUnit);
                    print("New Power: " + GetOffensivePower(frontRowSimulated, backRowSimulated));
                    int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);
                    float utility = (GetOffensivePower(frontRowSimulated, backRowSimulated) - currentPower) / unitCost;
                    print("utility: " + utility);
                    if (utility > maxUtility) {
                        maxUtility = utility;
                        armyToImprove = army;
                        unitIndex = i;
                    }
                }
            }
            // If allocated money
            else {
                print("allocated money, let's try upgrading");
                MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().frontRow);
                MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().backRow);
                foreach(MapUnit simulatedUnit in army.GetComponent<Army>().unitsToAdd) {
                    Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
                }
                float currentPower = GetOffensivePower(frontRowSimulated, backRowSimulated);
                bool openUnitSlot = false;
                if (Tools.RowsHaveOpening(frontRowSimulated, backRowSimulated)) openUnitSlot = true;
                for (int i = 0; i < 4; i++) {
                    if (i == 2) i = 3;
                    print("Index: " + i);
                    print("Current Power: " + currentPower);
                    MapUnit[] frontRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(frontRowSimulated);
                    MapUnit[] backRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(backRowSimulated);
                    MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                    if (openUnitSlot) {
                        print("there's an open spot: "+ Tools.AddUnitToRows(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, newUnit));
                    }
                    else Tools.ReplaceWeakestUnitInRows(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, newUnit);
                    int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);
                    if (!openUnitSlot) {
                        if (i == 1) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(0);
                        if (i == 3) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(1);
                    }
                    print("New Power: " + GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade));
                    float utility = (GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade) - currentPower) / unitCost;
                    print("utility: " + utility);
                    if (utility > maxUtility) {
                        maxUtility = utility;
                        armyToImprove = army;
                        unitIndex = i;
                    }
                }
            }
        }
        print("Max Utility = " + maxUtility);
        print("Army To Improve = " + armyToImprove.name);
        print("Unit Index = " + unitIndex);
        return maxUtility;
    }
    float GetBuildingUtility() {
        float templeArmamentsUtility = 0;
        return templeArmamentsUtility;
    }

    void BuyUnits() {
        //print("army count: "+armies.Count);
        bool moneyToSpend = true;
        while (moneyToSpend) {
            //print("has " + GetComponent<Player>().money + " money to spend");
            float greatestPowerDifference = 9999;
            GameObject weakestArmy = armies[0];
            for (int i = 0; i < armies.Count; i++) {
                GameObject armyNode = armies[i].GetComponent<Army>().currentNode;
                float powerDifference = armies[i].GetComponent<Army>().GetOffensivePower() - armyNode.GetComponent<Node>().GetThreatToNode();
                if (powerDifference < greatestPowerDifference) {
                    weakestArmy = armies[i];
                    greatestPowerDifference = powerDifference;
                }
            }
            if (weakestArmy.GetComponent<Army>().HasOpenPosition()) {
                //print("weakest has opening");
                UnitPos position = weakestArmy.GetComponent<Army>().GetOpenPosition();
                MapUnit unit = GetComponent<Player>().unitBlueprints[3];
                weakestArmy.GetComponent<Army>().BuyUnit(position, unit);
                if (GetComponent<Player>().money < unit.moneyCost) moneyToSpend = false;
            }
            else moneyToSpend = false;
        }
    }
    public float GetDefensivePower(MapUnit[] frontRowUnits, MapUnit[] backRowUnits, Temple temple, Altar altar) {
        float totalHealth = 0;
        float totalDPS = 0;
        float power = 0;
        if (temple != null && temple.name == TempleName.Protection) {
            totalHealth += temple.unit.GetHealth();
            power += temple.unit.GetDPS() * totalHealth;
        }
        if (altar != null && altar.name == AltarName.Conflict) {
            totalHealth += altar.unit.GetHealth();
            power += altar.unit.GetDPS() * totalHealth;
        }
        for (int i = 0; i < frontRowUnits.Length; i++) {
            if (frontRowUnits[i] != null) {
                totalHealth += frontRowUnits[i].GetHealth();
                totalDPS += frontRowUnits[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        totalDPS = 0;
        for (int i = 0; i < backRowUnits.Length; i++) {
            if (backRowUnits[i] != null) {
                totalHealth += backRowUnits[i].GetHealth();
                totalDPS += backRowUnits[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        return power;
    }
    public float GetOffensivePower(MapUnit[] frontRowUnits, MapUnit[] backRowUnits) {
        float totalHealth = 0;
        float totalDPS = 0;
        float power = 0;
        for (int i = 0; i < frontRowUnits.Length; i++) {
            if (frontRowUnits[i] != null) {
                totalHealth += frontRowUnits[i].GetHealth();
                totalDPS += frontRowUnits[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        totalDPS = 0;
        for (int i = 0; i < backRowUnits.Length; i++) {
            if (backRowUnits[i] != null) {
                totalHealth += backRowUnits[i].GetHealth();
                totalDPS += backRowUnits[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        return power;
    }
    int GetFrontierNodeCount() {
        int frontierNodes = 0;
        foreach (GameObject node in ownedNodes) {
            bool nodeIsOnFrontier = false;
            foreach (GameObject neighbour in node.GetComponent<Node>().neighbours) {
                if (neighbour.GetComponent<Node>().faction != Faction.Independent && neighbour.GetComponent<Node>().faction != node.GetComponent<Node>().faction) {
                    nodeIsOnFrontier = true;
                }
            }
            if (nodeIsOnFrontier) frontierNodes++;
        }
        return frontierNodes;
    }


    void Attack() {
        moreToDoInPhase = false;
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (AttackNearbyThreat(currentArmy)) {
                //print("Attack performed, waiting");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
        for (int i = 0; i < armies.Count; i++) {
            GameObject currentArmy = armies[i];
            if (AttackNeutral(currentArmy)) {
                print("Attacking Neutral");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
    }
    bool AttackNeutral(GameObject army) {
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        GameObject target = armyNode.GetComponent<Node>().GetGreatestNeutralThreat();
        if (target != null && ShouldAttack(army, target)) {
            GetComponent<Player>().attackNode(army, target);
            return true;
        }
        return false;
    }
    bool AttackNearbyThreat(GameObject army) {
        //print("checking attacks");
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        if (KnowsAllNearbyThreats(army)) {
            GameObject target = GetGreatestThreat(army);
            if (target != null && ShouldAttack(army, target.GetComponent<Army>().currentNode)) {
                //print("ATTACK!");
                GetComponent<Player>().attackNode(army, target.GetComponent<Army>().currentNode);
                return true;
            }
        }
        return false;
    }
    bool ShouldAttack(GameObject attackingArmy, GameObject defendingNode) {
        //print("attack power: " + attackingArmy.GetComponent<Army>().GetOffensivePower());
        //print("defend power: " + defendingNode.GetComponent<Node>().GetDefensivePower());
        if (attackingArmy.GetComponent<Army>().GetOffensivePower() >= 1.2 * defendingNode.GetComponent<Node>().GetDefensivePower()) {
            return true;
        }
        return false;
    }
    GameObject GetGreatestThreat(GameObject army) {
        List<GameObject> neighbouringNodes = army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours;
        GameObject mostThreateningEnemy = null;
        float highestThreat = 0;
        for (int i = 0; i < neighbouringNodes.Count; i++) {
            GameObject neighbourArmy = neighbouringNodes[i].GetComponent<Node>().occupant;
            if (neighbourArmy && neighbourArmy.GetComponent<Army>().faction != faction && neighbourArmy.GetComponent<Army>().faction != Faction.Independent) {
                if (enemyArmyInfo.ContainsKey(neighbourArmy) && enemyArmyInfo[neighbourArmy].expectedPower > highestThreat) {
                    mostThreateningEnemy = neighbourArmy;
                    highestThreat = enemyArmyInfo[neighbourArmy].expectedPower;
                }
            }
        }
        return mostThreateningEnemy;
    }
    bool KnowsAllNearbyThreats(GameObject army) {
        List<GameObject> neighbouringNodes = army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbouringNodes.Count; i++) {
            GameObject neighbourArmy = neighbouringNodes[i].GetComponent<Node>().occupant;
            if (neighbourArmy && neighbourArmy.GetComponent<Army>().faction != faction && neighbourArmy.GetComponent<Army>().faction != Faction.Independent) {
                if (!enemyArmyInfo.ContainsKey(neighbourArmy) || enemyArmyInfo.ContainsKey(neighbourArmy) && enemyArmyInfo[neighbourArmy].timeSinceScout >= 2) {
                    print("We don't know everybody");
                    return false;
                }
            }
        }
        return true;
    }





    void TestIDS() {
        for (int k = 0; k < 1; k++) {
            for (int i = 0; i < armies.Count; i++) {
                // For all armies
                GameObject army = armies[i];
                GameObject armyNode = army.GetComponent<Army>().currentNode;
                GameObject randomNeighbour = armyNode.GetComponent<Node>().GetRandomNeighbour2();
                List<GameObject> pathway = armyNode.GetComponent<Node>().GetPathTo(randomNeighbour);
                //print("Pathway to random neighbour: " + pathway);
                for (int j = 0; j < pathway.Count; j++) {
                    //print("Path includes: " + pathway[j]);
                }
            }
        }
    }
    public void StartTurn() {
        turnPhase = TurnPhase.DefendAgainstThreats;
        readyToExecute = true;
        unallocatedMoney = GetComponent<Player>().money;
    }
    void IncrementArmyInfo() {
        foreach (KeyValuePair<GameObject, ArmyInfo> info in enemyArmyInfo) {
            ArmyInfo newInfo = info.Value;
            newInfo.timeSinceScout++;
            enemyArmyInfo[info.Key] = newInfo;
        }
    }
    bool readyForNextPhase() {
        if (!moreToDoInPhase && Army.readyToMove) return true;
        return false;
    }

    /*

    float ThreatToNode(GameObject node) {
        float threat = 0;
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction != faction) {
                threat = Mathf.Max(threat, neighbour.GetComponent<Node>().occupant.GetComponent<Army>().GetOffensivePower());
            }
        }
        return threat;
    }

    void ExecuteIntent() {
        print("Executing Intent");
        readyToExecute = false;
        Intent intent = intentQueue[0];
        switch (intent.type) {
            case "Attack":
                print("execute attack");
                GetComponent<Player>().attackNode(intent.army, intent.node);
                break;
            case "Move":
                print("execute move");
                GetComponent<Player>().attackNode(intent.army, intent.node);
                break;
        }
        intentQueue.RemoveAt(0);

    }

    void CreateIntent(string intType, GameObject army, GameObject node) {
        print("Creating Intent");
        print("Intent Name: " + intType);
        print("Intent Army: " + army);
        print("Intent Target: " + node);
        Intent intent = new Intent();
        intent.type = intType;
        intent.army = army;
        intent.node = node;
        intentQueue.Add(intent);
    }
    */

    /*
void MoveToThreatened(List<GameObject> nodesByThreat) {
    //TestIDS();
    for (int i = 0; i < nodesByThreat.Count; i++) {
        //print(nodesByThreat[i] + ": " + nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
    }

    for (int i = 0; i < armies.Count; i++) {
        // For all armies
        GameObject army = armies[i];
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        List<GameObject> nodesInRange = new List<GameObject>();
        nodesInRange = GetNodesInRange(nodesInRange, armyNode, army.GetComponent<Army>().movesLeft);

        GameObject reachableThreatenedNode = null;
        int j = nodesByThreat.Count - 1;
        // Search nodes by threat descending, choose a reachable node
        while (reachableThreatenedNode == null) {
            GameObject node = nodesByThreat[j];
            if (nodesInRange.Contains(node)) {
                reachableThreatenedNode = node;
            }
            j--;
        }
        // If reachable node is found, attack that node
        if (reachableThreatenedNode) {
            //print("Found and moving to: " + reachableThreatenedNode);
            CreateIntent("Move", army, reachableThreatenedNode);
            //GetComponent<Player>().attackNode(army, reachableThreatenedNode);
            //print("Moves left: " + army.GetComponent<Army>().movesLeft);
            nodesByThreat.Remove(reachableThreatenedNode);
        }
    }
}

void AttackLargestThreat() {
    for (int i = 0; i < armies.Count; i++) {
        GameObject army = armies[i];
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        GameObject target = armyNode.GetComponent<Node>().GetGreatestThreat();
        if (target!= null) {
            CreateIntent("Attack", army, target);
            //GetComponent<Player>().attackNode(army, target);
        }
    }
}
*/

    // Deprecated
    /*
    GameObject GetMostThreatenedNeighbour(GameObject startNode, int range) {
        GameObject mostThreatened = startNode;
        int threat = ThreatToNode(startNode);
        if (range > 0) {
            for (int i = 0; i < startNode.GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = startNode.GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().faction == faction) {

                }
            }
        }
    }
    
    void CompileOptionsStage() {
        print("are you even alive? 1");
        BuyUnits();

        List<GameObject> nodesByThreat = ownedNodes;
        nodesByThreat.Sort(Tools.SortByThreat);
        MoveToThreatened(nodesByThreat);

        AttackLargestThreat();
        turnStage = 1;
    }
    void ExecutionStage() {
        print("are you even alive? 2");
        if (intentQueue.Count != 0 && readyToExecute) {
            ExecuteIntent();
        }

        if (intentQueue.Count == 0) turnStage = 2;
    }
    */
}
