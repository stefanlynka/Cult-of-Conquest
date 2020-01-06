using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<Intent> intentQueue = new List<Intent>();
    public Dictionary<GameObject, ArmyInfo> enemyArmyInfo = new Dictionary<GameObject, ArmyInfo>();
    GameObject turnManager;
    List<AllocateOption> bestOptions = new List<AllocateOption>();
    List<AllocateOption> committedOptions = new List<AllocateOption>();

    Faction faction;
    public TurnPhase turnPhase = TurnPhase.DefendAgainstThreats;
    public bool readyToExecute = true;
    bool moreToDoInPhase = false;
    int unallocatedMoney = 0;
    int investInProphets = 0;
    int endTurnTimer = 0;
    int endTurnMax = 100;
    //int investInBuildings = 0;


    // Start is called before the first frame update
    void Start() {
        CollectInformation();
    }

    void CollectInformation() {
        armies = GetComponent<Player>().armies;
        ownedNodes = GetComponent<Player>().ownedNodes;
        faction = GetComponent<Player>().faction;
        turnManager = GameObject.Find("/Turn Manager");
    }

    // Update is called once per frame

    void Update() {
        //print("Ready to Execute = :" + readyToExecute);
        //print("Armies are ready: " + Army.readyToMove);
        if (TurnManager.currentPlayer == gameObject) {
            if (readyToExecute && Army.readyToMove && Army.movesToDo.Count == 0) {
                if (turnPhase == TurnPhase.DefendAgainstThreats) {
                    //print("Defending Phase");
                    endTurnTimer = 0;
                    Defend();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Scouting;
                }
                else if (turnPhase == TurnPhase.Scouting) {
                    //print("Scouting Phase");
                    Scout();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Spending1;
                }
                else if (turnPhase == TurnPhase.Spending1) {
                    //print("Spending 1 Phase");
                    AllocateMoney();
                    SpendMoney();
                    SpendZeal();
                    turnPhase = TurnPhase.Attacks1;
                }
                else if (turnPhase == TurnPhase.Attacks1) {
                    //print("Attacking 1 Phase");
                    Scout();
                    MoveToFrontier();
                    Attack();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Spending2;
                }
                else if (turnPhase == TurnPhase.Spending2) {
                    //print("Spending 2 Phase");
                    SpendSavedMoney();
                    AllocateMoney();
                    SpendMoney();
                    SpendZeal();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Attacks2;
                }
                else if (turnPhase == TurnPhase.Attacks2) {
                    //print("Attacking 2 Phase");
                    Scout();
                    Attack();
                    MoveToFrontier();
                    Attack();
                    if (readyForNextPhase()) turnPhase = TurnPhase.Done;
                }
                else if (turnPhase == TurnPhase.Done) {
                    endTurnTimer++;
                    endTurnMax = 30 + 5 * armies.Count;
                    if (Army.movesToDo.Count==0 && endTurnTimer>endTurnMax) turnManager.GetComponent<TurnManager>().NextTurn();
                    //print("Turn Over");
                }

            }
        }
    }

    public bool WantsToFight(GameObject army, GameObject defendingNode) {
        if (army.GetComponent<Army>().GetOffensivePower() >= 1.25f * defendingNode.GetComponent<Node>().GetDefensivePower()) {
            //print("AI attacking power: " + army.GetComponent<Army>().GetOffensivePower());
            //print("Human defending power: " + defendingNode.GetComponent<Node>().GetDefensivePower());
            return true;
        }
        return false;
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
            else if (nodesByThreat.Contains(node) && node.GetComponent<Node>().GetThreatToNode() <= 0) {
                nodesByThreat.Remove(node);
                index--;
            }
            index++;
        }

        for (int i = 0; i < nodesByThreat.Count; i++) {
            //print("nodesByThreat: " + nodesByThreat[i].GetComponent<Node>().GetThreatToNode());
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
            //print("found someone worth protecting :')");
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
                if (!enemyArmyInfo.ContainsKey(neighbouringArmy) || (enemyArmyInfo.ContainsKey(neighbouringArmy) && enemyArmyInfo[neighbouringArmy].timeSinceScout >= 3)) {
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
        investInProphets = 0;
        unallocatedMoney = GetComponent<Player>().money;
        //print("unallocated money at start $" + unallocatedMoney);
        //print("Unallocated Money (before matching): " + unallocatedMoney);
        //Dictionary<GameObject, int> armyRequirements = new Dictionary<GameObject, int>();
        foreach (GameObject army in armies) {
            int moneyNeeded = GetMinMoneyNeeded(army);
            totalMoneyNeeded += moneyNeeded;
            //armyRequirements.Add(army, moneyNeeded);
            //army.GetComponent<Army>().allocatedMoney = moneyNeeded;
            //print("money needed: " + moneyNeeded);
        }
        //print("Unallocated Money (after matching): " + unallocatedMoney);
        //else print("Have enough money to allocate");
        //print("Total Money Needed: " + totalMoneyNeeded);
        while (unallocatedMoney >= 5) {
            //print("new option");
            //print("unallocated money = " + unallocatedMoney);
            bestOptions.Clear();
            GetProphetUtility();
            GetArmyUtility();
            GetReplenishUtility();
            GetBuildingUtility();
            bestOptions.Sort(Tools.SortByUtility);
            if (bestOptions.Count > 0) {
                CommitBestOption(bestOptions);
            }
            else unallocatedMoney = 0;
        }
        foreach (AllocateOption option in committedOptions) {
            //print("Option: " + option.type);
        }
        //print("All money allocated");
    }
    void SpendMoney() {
        ExecuteOptions(committedOptions);
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
                //print("Building " + unitsNeeded.Count + " units will be enough");
                for (int i = 0; i < unitsNeeded.Count; i++) {
                    moneyNeeded += unitsNeeded[i].moneyCost;
                    //print("this unit cost: " + unitsNeeded[i].moneyCost);
                }
                army.GetComponent<Army>().unitsToAdd = unitsNeeded;
                UnitsToAllocateOptions(army, unitsNeeded);
                return moneyNeeded;
            }

            unitsNeeded = UnitsNeededByCopyingMax(frontRowDefault, backRowDefault, templeDefault, altarDefault, threat);
            if (unitsNeeded.Count > 0) {
                //print("Building the biggest unit will be enough");
                for (int i = 0; i < unitsNeeded.Count; i++) {
                    moneyNeeded += unitsNeeded[i].moneyCost;
                }
                army.GetComponent<Army>().unitsToAdd = unitsNeeded;
                UnitsToAllocateOptions(army, unitsNeeded);
                return moneyNeeded;
            }

        }
        //else print("Don't need money");
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
        //print("numOpenSpots " + numOpenSpots);
        while (index <= enemyUnits.Count && index <= numOpenSpots && GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) < threatLevel) {
            //print("copying enemy unit");
            MapUnit simulatedUnit = GetComponent<Player>().unitBlueprints[Tools.UnitToIndex(enemyUnits[enemyUnits.Count-index])].DeepCopy();
            Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
            unitsNeeded.Add(simulatedUnit);
            index++;
        }
        if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
            //print("Defensive Power: " + GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated));
            //print("Enemy Power: " + threatLevel);
            return unitsNeeded;
        }

        if (templeSimulated == null) {
            //print("build a temple");
            templeSimulated = GetComponent<Player>().templeBlueprints[TempleName.Protection].DeepCopy();
            MapUnit templeStandIn = new MapUnit("Temple", Faction.None, "");
            templeStandIn.moneyCost = 30;//GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            unitsNeeded.Add(templeStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }
        if (altarSimulated == null) {
            //print("build an altar");
            altarSimulated = GetComponent<Player>().altarBlueprints[AltarName.Conflict].DeepCopy();
            MapUnit altarStandIn = new MapUnit("Altar", Faction.None, "");
            altarStandIn.moneyCost = 20;//GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
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
            MapUnit templeStandIn = new MapUnit("Temple", Faction.None, "");
            templeStandIn.moneyCost = 30;//GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            unitsNeeded.Add(templeStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }
        if (altarSimulated == null) {
            altarSimulated = GetComponent<Player>().altarBlueprints[AltarName.Conflict].DeepCopy();
            MapUnit altarStandIn = new MapUnit("Altar", Faction.None, "");
            altarStandIn.moneyCost = 20;// GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
            unitsNeeded.Add(altarStandIn);
            if (GetDefensivePower(frontRowSimulated, backRowSimulated, templeSimulated, altarSimulated) > threatLevel) {
                return unitsNeeded;
            }
        }

        return new List<MapUnit>();
    }

    void GetProphetUtility() {
        if (unallocatedMoney >= 20 && investInProphets == 0 && ownedNodes.Count > armies.Count) {
            float localNodesToConquer = 8;
            float expandingUtility = localNodesToConquer / armies.Count + 0.1f;
            //print("expanding Util: " + expandingUtility);

            float protectBordersUtility = Mathf.Pow((GetFrontierNodeCount() / armies.Count + 0.1f)-1,2);
            //print("protectborder Util: " + protectBordersUtility);
            float utility = Mathf.Max(expandingUtility, protectBordersUtility);
            AllocateOption option = new AllocateOption(OptionType.Prophet, utility, 30);
            bestOptions.Add(option);
            //print("Prophet Utility: " + utility);
            return;
        }
        else return;
    }
    void GetArmyUtility() {
        float maxUtility = 0;
        GameObject armyToImprove = null;
        int newUnitIndex = 0;
        //int bestNewIndex = 0;
        int bestUnitCost = 0;
        OptionType buildType = OptionType.None;
        UnitPos bestUnitPos = new UnitPos();
        int oldUnitIndex = 0;

        foreach (GameObject army in armies) {
            /*
            if (army.GetComponent<Army>().allocatedMoney == 0) {
                float currentPower = army.GetComponent<Army>().GetOffensivePower();
                for (int i = 0; i < 4; i++) {
                    if (i == 2) i = 3;
                    //print("Index: " + i);
                    //print("Current Power: " + currentPower);
                    MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().frontRow);
                    MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().backRow);
                    MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                    if (army.GetComponent<Army>().HasOpenPosition()) Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, newUnit);
                    else Tools.ReplaceWeakestUnitInRows(frontRowSimulated, backRowSimulated, newUnit);
                    //print("New Power: " + GetOffensivePower(frontRowSimulated, backRowSimulated));
                    int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);
                    float utility = (15 * GetOffensivePower(frontRowSimulated, backRowSimulated) / currentPower) / unitCost;
                    //print("Unit utility: " + utility);
                    if (utility > maxUtility && unallocatedMoney >= unitCost) {
                        maxUtility = utility;
                        armyToImprove = army;
                        unitIndex = i;
                        bestUnitCost = unitCost;
                    }
                }
            }
            */
            // If allocated money
            //print("allocated money, let's try upgrading");
            MapUnit[] frontRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().frontRow);
            MapUnit[] backRowSimulated = Tools.DeepCopyMapUnitArray(army.GetComponent<Army>().backRow);
            List<MapUnit> simulatedUnits = new List<MapUnit>();
            foreach(AllocateOption option in committedOptions) {
                if (option.type == OptionType.Unit && option.army == army) {
                    MapUnit simulatedUnit = GetComponent<Player>().unitBlueprints[option.unitIndex].DeepCopy();
                    simulatedUnits.Add(simulatedUnit);
                    Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
                }
            }
            /*
            foreach(MapUnit simulatedUnit in army.GetComponent<Army>().unitsToAdd) {
                Tools.AddUnitToRows(frontRowSimulated, backRowSimulated, simulatedUnit);
            }
            */
            float currentPower = GetOffensivePower(frontRowSimulated, backRowSimulated);
            bool openUnitSlot = false;
            if (Tools.RowsHaveOpening(frontRowSimulated, backRowSimulated)) openUnitSlot = true;
            // If there's an open slot, try adding each unit
            if (openUnitSlot) {
                for (int i = 0; i < 4; i++) {
                    if (i == 2) i = 3;
                    //print("Index: " + i);
                    //print("Current Power: " + currentPower);
                    MapUnit[] frontRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(frontRowSimulated);
                    MapUnit[] backRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(backRowSimulated);
                    MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                    Tools.AddUnitToRows(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, newUnit);

                    int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);

                    //print("New Power: " + GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade));
                    float utility = (15 * GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade) / currentPower) / unitCost;
                    //print("utility: " + utility);
                    if (utility > maxUtility && unallocatedMoney >= unitCost) {
                        maxUtility = utility;
                        armyToImprove = army;
                        newUnitIndex = i;
                        bestUnitCost = unitCost;
                        buildType = OptionType.Unit;
                    }
                }
            }
            // if there's no open slot, try upgrading each simulated unit or replacing every real unit
            else {
                foreach (MapUnit unitToBeReplaced in frontRowSimulated) {
                    int unitToBeReplacedIndex = Tools.UnitToIndex(unitToBeReplaced);
                    for (int i = 1; i < 4; i++) {
                        if (i == 2) i = 3;
                        MapUnit[] frontRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(frontRowSimulated);
                        MapUnit[] backRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(backRowSimulated);
                        MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                        bool replacingSimulated = simulatedUnits.Contains(unitToBeReplaced);
                        Tools.ReplaceUnit(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, unitToBeReplaced, newUnit);
                        int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);
                        if (replacingSimulated) {
                            if (i == 1) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(1) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(0);
                            if (i == 3) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(3) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(1);
                        }
                        float utility = 15 * (GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade) - currentPower) / unitCost;
                        if (utility > maxUtility && unallocatedMoney >= unitCost) {
                            maxUtility = utility;
                            armyToImprove = army;
                            newUnitIndex = i;
                            bestUnitCost = unitCost;
                            if (replacingSimulated) {
                                buildType = OptionType.Upgrade;
                                oldUnitIndex = Tools.UnitToIndex(unitToBeReplaced);
                            }
                            else {
                                buildType = OptionType.Replace;
                                bestUnitPos = Tools.GetUnitPosition(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, unitToBeReplaced);
                            }
                        }
                    }
                }
                foreach (MapUnit unitToBeReplaced in backRowSimulated) {
                    for (int i = 1; i < 4; i++) {
                        if (i == 2) i = 3;
                        MapUnit[] frontRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(frontRowSimulated);
                        MapUnit[] backRowSimulatedUpgrade = Tools.DeepCopyMapUnitArray(backRowSimulated);
                        MapUnit newUnit = GetComponent<Player>().unitBlueprints[i];
                        bool replacingSimulated = simulatedUnits.Contains(unitToBeReplaced);
                        Tools.ReplaceUnit(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, unitToBeReplaced, newUnit);
                        int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(i);
                        if (replacingSimulated) {
                            if (i == 1) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(1) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(0);
                            if (i == 3) unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(3) - army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(1);
                        }
                        float utility = 15 * (GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade) - currentPower) / unitCost;
                        if (utility > maxUtility && unallocatedMoney >= unitCost) {
                            print("Current Power: " + currentPower);
                            print("New Power: " + GetOffensivePower(frontRowSimulatedUpgrade, backRowSimulatedUpgrade));
                            print("Unit Cost: " + unitCost);
                            print("Utility: " + utility);
                            maxUtility = utility;
                            armyToImprove = army;
                            newUnitIndex = i;
                            bestUnitCost = unitCost;
                            if (replacingSimulated) {
                                buildType = OptionType.Upgrade;
                                oldUnitIndex = Tools.UnitToIndex(unitToBeReplaced);
                            }
                            else {
                                buildType = OptionType.Replace;
                                bestUnitPos = Tools.GetUnitPosition(frontRowSimulatedUpgrade, backRowSimulatedUpgrade, unitToBeReplaced);
                            }
                        }
                    }
                }
            }
        }
        if (armyToImprove != null) {
            if (buildType == OptionType.Unit) {
                AllocateOption option = new AllocateOption(buildType, maxUtility, bestUnitCost, armyToImprove, newUnitIndex);
                bestOptions.Add(option);
            }
            else if (buildType == OptionType.Upgrade) {
                AllocateOption option = new AllocateOption(buildType, maxUtility, bestUnitCost, armyToImprove, newUnitIndex, oldUnitIndex);
                bestOptions.Add(option);
            }
            else if (buildType == OptionType.Replace) {
                AllocateOption option = new AllocateOption(buildType, maxUtility, bestUnitCost, armyToImprove, newUnitIndex, bestUnitPos);
                bestOptions.Add(option);
            }
            //print("Max Utility = " + maxUtility);
            //print("Army To Improve = " + armyToImprove.name);
            //print("Unit Index = " + unitIndex);
        }

        return;
    }
    void GetReplenishUtility() {
        float maxUtility = 0;
        GameObject bestArmyToReplenish = null;
        bool firstReplenish = true;
        foreach(GameObject army in armies) {
            GameObject enemyWorthAttacking = EnemyWorthAttacking(army);
            if (enemyWorthAttacking != null){
                //print("Enemy Worth Attacking " + enemyWorthAttacking.name);
                GameObject strongestAlly = GetStrongestEnemyAlly(enemyWorthAttacking);
                if (strongestAlly) {
                    //print("Strongest Enemy Ally " + strongestAlly.name);
                    if (ArmyReplenishCount(army) == 0 && unallocatedMoney >= 30) {
                        maxUtility = (enemyArmyInfo[strongestAlly].expectedPower / army.GetComponent<Army>().GetOffensivePower()) * 7;
                        bestArmyToReplenish = army;
                        firstReplenish = true;
                    }
                    else if (ArmyReplenishCount(army) == 1 && unallocatedMoney >= 20) {
                        maxUtility = (enemyArmyInfo[strongestAlly].expectedPower / army.GetComponent<Army>().GetOffensivePower()) * 5;
                        bestArmyToReplenish = army;
                        firstReplenish = false;
                    }
                }
            }
        }
        if (maxUtility > 0) {
            int replenishCost = 0;
            if (firstReplenish) replenishCost = 30;
            else if (!firstReplenish) replenishCost = 20;
            AllocateOption option = new AllocateOption(OptionType.Replenish, maxUtility, replenishCost, bestArmyToReplenish);
            bestOptions.Add(option);
        }
        //print("Replenish Utility: " + maxUtility);
        return;
    }
    void GetBuildingUtility() {
        Dictionary<AltarName, Altar> altarDict = GetComponent<Player>().altarBlueprints;
        Dictionary<TempleName, Temple> templeDict = GetComponent<Player>().templeBlueprints;
        if (unallocatedMoney >= templeDict[TempleName.Armaments].cost) GetTempleArmamentsUtility();
        if (unallocatedMoney >= templeDict[TempleName.Tradition].cost) GetTempleTraditionUtility();
        if (unallocatedMoney >= templeDict[TempleName.Origin].cost) GetTempleOriginUtility();
        if (unallocatedMoney >= altarDict[AltarName.Devotion].cost) GetAltarDevotionUtility();
        if (unallocatedMoney >= altarDict[AltarName.Harvest].cost) GetAltarHarvestUtility();

        return;
    }
    void GetTempleArmamentsUtility() {
        GameObject bestArmy = null;
        int mostMoneyToBeSpent = 0;
        foreach (GameObject army in armies) {
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            if (armyNode.GetComponent<Node>().temple == null) {
                int moneyToBeSpent = 0;
                foreach (AllocateOption option in committedOptions) {
                    if (option.type == OptionType.Unit && option.army == army) {
                        moneyToBeSpent += army.GetComponent<Army>().owner.GetComponent<Player>().unitBlueprints[option.unitIndex].moneyCost;
                    }
                }
                if (moneyToBeSpent > mostMoneyToBeSpent) {
                    mostMoneyToBeSpent = moneyToBeSpent;
                    bestArmy = army;
                }
            }
        }
        if (bestArmy != null) {
            float utility = mostMoneyToBeSpent / 7.5f;
            //print("temple Armaments Utility: " + utility);
            AllocateOption option = new AllocateOption(OptionType.Temple, utility, 30, bestArmy.GetComponent<Army>().currentNode, TempleName.Armaments);
            bestOptions.Add(option);
        }
        return;
    }
    void GetTempleTraditionUtility() {
        //float utility = 0;
        //print("temple Tradition Utility: " + utility);
        return;
    }
    void GetTempleOriginUtility() {
        GameObject bestNode = GetBestOriginNode();
        if (!HasTempleOfOrigin() && !PlanToBuildOrigin()) {
            AllocateOption option = new AllocateOption(OptionType.Temple, 7.5f, 50, bestNode, TempleName.Origin);
            bestOptions.Add(option);
        }
        //print("temple Origin Utility: " + utility);
        return;
    } 
    void GetAltarDevotionUtility() {
        GameObject bestNode = GetBestDevotionNode();
        float utility = 6f;
        //print("altar Devotion Utility: " + utility);
        if (bestNode) {
            AllocateOption option = new AllocateOption(OptionType.Altar, utility, 20, bestNode, AltarName.Devotion);
            bestOptions.Add(option);
        }
        return;
    }
    void GetAltarHarvestUtility() {
        GameObject bestNode = GetBestHarvestNode();
        if (bestNode != null) {
            float utility = (bestNode.GetComponent<Node>().GetNodeMoneyIncome()-2) / 1.5f;
            //print("altar Harvest Utility: " + utility);
            AllocateOption option = new AllocateOption(OptionType.Altar, utility, 20, bestNode, AltarName.Harvest);
            bestOptions.Add(option);
        }
        return;
    }

    void CommitBestOption(List<AllocateOption> options) {
        //print("Unallocated Money: " + unallocatedMoney);
        //print("Best Option: " + options[bestOptions.Count - 1].type + " utility: " + options[bestOptions.Count - 1].utility);
        //print("Worst Option: " + options[0].type + " utility: "+ options[0].utility);
        AllocateOption bestOption = options[bestOptions.Count - 1];
        //print("Best Option: " + bestOption.type);
        if (bestOption.type != OptionType.Upgrade) committedOptions.Add(bestOption);
        //if (bestOption.type == "prophet") investInProphets += 30;
        if (bestOption.type == OptionType.Upgrade) Replace(bestOption);

        unallocatedMoney -= bestOption.cost;
    }
    void ExecuteOptions(List<AllocateOption> options) {
        //print(options.Count + " Actions to Execute");
        //print("Money Before: " + GetComponent<Player>().money);
        //print("Unallocated Money Before: %" + unallocatedMoney);
        while (committedOptions.Count > 0) {
            AllocateOption option = options[0];
            committedOptions.RemoveAt(0);
            print("Utility: " + option.utility);
            switch (option.type) {
                case OptionType.Unit:
                    print("Building unit: " + GetComponent<Player>().unitBlueprints[option.unitIndex].name + " for Army: " + option.army.name);
                    option.army.GetComponent<Army>().BuyUnit(option.army.GetComponent<Army>().GetOpenPosition(), GetComponent<Player>().unitBlueprints[option.unitIndex]);
                    break;
                case OptionType.Replace:
                    print("Replacing Unit with "+ GetComponent<Player>().unitBlueprints[option.unitIndex].name + " in Army: "+ option.army + " at position "+ option.unitPos.position + " in frontrow " + option.unitPos.frontRow);
                    option.army.GetComponent<Army>().BuyUnit(option.unitPos, GetComponent<Player>().unitBlueprints[option.unitIndex]);
                    break;
                case OptionType.Replenish:
                    print("Saving " + option.cost + " for replenishing");
                    option.army.GetComponent<Army>().allocatedReplenish += option.cost;
                    break;
                case OptionType.Prophet:
                    print("Saving " + option.cost + " for prophet");
                    investInProphets += 30;
                    break;
                case OptionType.Temple:
                    print("Building Temple of " + option.templeName);
                    GetComponent<Player>().BuyTemple(option.node, option.templeName);
                    break;
                case OptionType.Altar:
                    print("Building Altar of " + option.altarName);
                    GetComponent<Player>().BuyAltar(option.node, option.altarName);
                    break;
            }
        }
        //print("Money After: $" + GetComponent<Player>().money);
        //print("Unallocated Money After: " + unallocatedMoney);
    }
    void Replace(AllocateOption upgradedOption) {
        foreach(AllocateOption oldOption in committedOptions) {
            if (oldOption.type == OptionType.Unit && oldOption.army == upgradedOption.army && oldOption.unitIndex == upgradedOption.oldUnitIndex) {
                //print("upgrade worked. From " + oldOption.unitIndex + " to " + upgradedOption.unitIndex);
                oldOption.unitIndex = upgradedOption.unitIndex;
                return;
            }
        }
    }
    GameObject GetBestOriginNode() {
        GameObject bestNode = null;
        foreach (GameObject army in GetComponent<Player>().armies) {
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            if (!PlanToUseNodesTemple(armyNode) && armyNode.GetComponent<Node>().temple == null) return armyNode;
        }
        foreach (GameObject node in ownedNodes) {
            if (node.GetComponent<Node>().temple == null) return node;
        }
        return bestNode;
    }
    GameObject GetBestHarvestNode() {
        GameObject bestNode = null;
        int highestIncome = 0;
        foreach (GameObject node in ownedNodes) {
            if (!PlanToUseNodesAltar(node) && node.GetComponent<Node>().altar == null) {
                int income = node.GetComponent<Node>().GetNodeMoneyIncome();
                if ((income > highestIncome && (bestNode == null || !(bestNode.GetComponent<Node>().occupant != null && node.GetComponent<Node>().occupant == null )))
                    || (income>=0.8*highestIncome && node.GetComponent<Node>().occupant != null && (bestNode == null || bestNode.GetComponent<Node>().occupant == null))) {
                    highestIncome = income;
                    bestNode = node;
                }
            }
        }
        return bestNode;
    }
    GameObject GetBestDevotionNode() {
        GameObject bestNode = null;
        int lowestIncome = 9999;
        foreach (GameObject army in GetComponent<Player>().armies) {
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            if (!PlanToUseNodesAltar(armyNode) && armyNode.GetComponent<Node>().altar == null) return armyNode;
        }
        foreach (GameObject node in ownedNodes) {
            if (!PlanToUseNodesAltar(node) && node.GetComponent<Node>().altar == null) {
                int income = node.GetComponent<Node>().GetNodeMoneyIncome();
                if (income < lowestIncome) {
                    lowestIncome = income;
                    bestNode = node;
                }
            }
        }
        return bestNode;
    }
    int ArmyReplenishCount(GameObject army) {
        int replenishCount = 0;
        foreach(AllocateOption option in bestOptions) {
            if (option.type == OptionType.Replenish && option.army == army) replenishCount++;
        }
        return replenishCount;
    }
    public float GetDefensivePower(MapUnit[] frontRowUnits, MapUnit[] backRowUnits, Temple temple, Altar altar) {
        float totalHealth = 0;
        float totalDPS = 0;
        float power = 0;
        if (temple != null) {
            totalHealth += temple.unit.GetHealth();
            power += temple.unit.GetDPS() * totalHealth;
        }
        if (altar != null) {
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
    GameObject EnemyWorthAttacking(GameObject army) {
        float highestStrength = 0;
        GameObject strongestNeighbour = null;
        foreach (GameObject neighbour in army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours) {
            if (neighbour.GetComponent<Node>().faction != faction && neighbour.GetComponent<Node>().faction != Faction.Independent && neighbour.GetComponent<Node>().occupant != null) {
                GameObject neighbourArmy = neighbour.GetComponent<Node>().occupant;
                if (enemyArmyInfo.ContainsKey(neighbourArmy) && enemyArmyInfo[neighbourArmy].expectedPower > highestStrength) {
                    //print("We found a strong enemy");
                    highestStrength = enemyArmyInfo[neighbourArmy].expectedPower;
                    strongestNeighbour = neighbourArmy;
                }
            }
        }
        if (strongestNeighbour != null && enemyArmyInfo.ContainsKey(strongestNeighbour) && army.GetComponent<Army>().GetOffensivePower() >= 1.25 * enemyArmyInfo[strongestNeighbour].expectedPower) {
            return strongestNeighbour;
        }
        return null;
    }
    GameObject GetStrongestEnemyAlly(GameObject enemyArmy) {
        float highestStrength = 0;
        GameObject strongestNeighbour = null;
        foreach (GameObject neighbour in enemyArmy.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours) {
            if (neighbour.GetComponent<Node>().occupant != null && neighbour.GetComponent<Node>().faction == enemyArmy.GetComponent<Army>().faction) {
                GameObject neighbourArmy = neighbour.GetComponent<Node>().occupant;
                if (enemyArmyInfo.ContainsKey(neighbourArmy) && enemyArmyInfo[neighbourArmy].expectedPower > highestStrength) {
                    highestStrength = enemyArmyInfo[neighbourArmy].expectedPower;
                    strongestNeighbour = neighbourArmy;
                }
            }
        }
        return strongestNeighbour;
    }
    void UnitsToAllocateOptions(GameObject army, List<MapUnit> units) {
        foreach (MapUnit unit in units) {
            int unitCost = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetUnitCost(Tools.UnitToIndex(unit));
            if (unit.name == "Temple") unitCost = GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            if (unit.name == "Altar") unitCost = GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
            if (unallocatedMoney >= unitCost) {
                if (unit.name == "Temple") {
                    AllocateOption option = new AllocateOption(OptionType.Temple, 0f, unitCost, army.GetComponent<Army>().currentNode, TempleName.Protection);
                    committedOptions.Add(option);
                    unallocatedMoney -= unitCost;
                }
                else if (unit.name == "Altar") {
                    AllocateOption option = new AllocateOption(OptionType.Altar, 0f, unitCost, army.GetComponent<Army>().currentNode, AltarName.Conflict);
                    committedOptions.Add(option);
                    unallocatedMoney -= unitCost;
                }
                else {
                    AllocateOption option = new AllocateOption(OptionType.Unit, 0f, unitCost, army, Tools.UnitToIndex(unit));
                    committedOptions.Add(option);
                    unallocatedMoney -= unitCost;
                }
            }
        }
    }
    bool HasTempleOfOrigin() {
        foreach (GameObject node in GetComponent<Player>().ownedNodes) {
            if (node.GetComponent<Node>().temple != null && node.GetComponent<Node>().temple.name == TempleName.Origin) return true;
        }
        return false;
    }
    bool PlanToBuildOrigin() {
        foreach(AllocateOption option in committedOptions) {
            if (option.type == OptionType.Temple && option.templeName == TempleName.Origin) return true;
        }
        return false;
    }
    bool PlanToUseNodesTemple(GameObject node) {
        foreach (AllocateOption option in committedOptions) {
            if (option.type == OptionType.Temple && option.node == node) return true;
        }
        return false;
    }
    bool PlanToUseNodesAltar(GameObject node) {
        foreach (AllocateOption option in committedOptions) {
            if (option.type == OptionType.Altar && option.node == node) return true;
        }
        return false;
    }



    void SpendZeal() {
        switch (faction) {
            case Faction.Noumenon:
                TryToUpgrade();
                break;
            case Faction.Zenteel:
                TryToUpgrade();
                break;
            case Faction.Paratrophs:
                TryToUpgrade();
                break;
            case Faction.Unmar:
                TryToUpgrade();
                break;
            case Faction.Samata:
                TryToUpgrade();
                break;
            case Faction.Carnot:
                TryToUpgrade();
                break;

        }
    }
    void TryToUpgrade() {
        foreach(KeyValuePair<string, Upgrade> upgrade in GetComponent<Player>().upgrades) {
            if (upgrade.Value.zealCost <= GetComponent<Player>().zeal && upgrade.Value.currentLevel < upgrade.Value.maxLevel) {
                GetComponent<Player>().BuyUpgrade(upgrade.Value);
                print("Bought Upgrade: "+ upgrade.Value.name);
            }
        }
    }
    void UpgradeParatrophs() {
        foreach (KeyValuePair<string, Upgrade> upgrade in GetComponent<Player>().upgrades) {
            if (upgrade.Value.zealCost <= GetComponent<Player>().zeal && upgrade.Value.currentLevel < upgrade.Value.maxLevel) {
                if (!upgrade.Value.name.Contains("Adapt")) GetComponent<Player>().BuyUpgrade(upgrade.Value);
                else if (upgrade.Value.name == "Adapt 1"){
                    Upgrade newUpgrade = GetRandomUpgrade();
                    GetComponent<Player>().upgrades.Remove("Adapt 1");
                    GetComponent<Player>().upgrades.Add(newUpgrade.name, newUpgrade);
                    GetComponent<Player>().BuyUpgrade(newUpgrade);
                }
                else if (upgrade.Value.name == "Adapt 2") {
                    Upgrade newUpgrade = GetRandomUpgrade();
                    GetComponent<Player>().upgrades.Remove("Adapt 2");
                    GetComponent<Player>().upgrades.Add(newUpgrade.name, newUpgrade);
                    GetComponent<Player>().BuyUpgrade(newUpgrade);
                }
            }
        }
    }
    Upgrade GetRandomUpgrade() {
        int randInt = Random.Range(0, 7);
        switch (randInt) {
            case 0:
                return new Upgrade("Strike First", 5, 3, "Increase damage \nwhen attacking");
            case 1:
                return new Upgrade("Cover Your Tracks", 8, 2, "Increase \nfog of war");
            case 2:
                return new Upgrade("Fortification", 5, 3, "Increase damage\nand health\n of protective buildings");
            case 3:
                return new Upgrade("Last One Standing", 5, 3, "Strengthen your\nlast unit\nin a battle");
            case 4:
                return new Upgrade("Against Tyranny", 5, 3, "Increase damage\nwhen attacking strongest\nenemy faction");
            case 5:
                return new Upgrade("Entropic Explorer", 5, 3, "Increase defense\nbased on\nnode's exposure");
            case 6:
                return new Upgrade("Defensive Discord", 5, 3, "Enemies have\na chance to \nattack randomly");
        }
        return null;
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
        foreach (GameObject currentArmy in armies) {
            if (AttackNearbyUnprotected(currentArmy)) {
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
                //print("Attacking Neutral");
                readyToExecute = false;
                moreToDoInPhase = true;
                //print("attacking, not ready to execute");
                return;
            }
        }
    }
    bool AttackNeutral(GameObject army) {
        //foreach()
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        GameObject armyNode = army.GetComponent<Army>().currentNode;
        List<GameObject> neutralThreats = armyNode.GetComponent<Node>().GetNeutralThreats();
        neutralThreats.Sort(Tools.SortByNodePower);
        neutralThreats.Reverse();
        foreach(GameObject neutralThreat in neutralThreats) {
            if (ShouldAttack(army, neutralThreat)) {
                GetComponent<Player>().attackNode(army, neutralThreat);
                return true;
            }
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
    bool AttackNearbyUnprotected(GameObject army) {
        if (army.GetComponent<Army>().movesLeft <= 0) return false;
        GameObject leastProtected = GetLeastProtectedEnemyNode(army);
        if (leastProtected && ShouldAttack(army, leastProtected)) {
            GetComponent<Player>().attackNode(army, leastProtected);
            return true;
        }

        return false;
    }
    GameObject GetLeastProtectedEnemyNode(GameObject army) {
        GameObject leastProtected = null;
        float leastProtectedPower = 9999f;
        foreach(GameObject neighbour in army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours) {
            if (EnemyNodeIsUnoccupied(neighbour)) {
                if (neighbour.GetComponent<Node>().GetDefensivePower() < leastProtectedPower) {
                    leastProtected = neighbour;
                    leastProtectedPower = neighbour.GetComponent<Node>().GetDefensivePower();
                }
            }
        }
        return leastProtected;
    }
    bool EnemyNodeIsUnoccupied(GameObject node) {
        if (node.GetComponent<Node>().faction != faction && node.GetComponent<Node>().faction != Faction.Independent && node.GetComponent<Node>().occupant == null) return true;
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
                if (!enemyArmyInfo.ContainsKey(neighbourArmy) || enemyArmyInfo.ContainsKey(neighbourArmy) && enemyArmyInfo[neighbourArmy].timeSinceScout >= 3) {
                    print("We don't know everybody");
                    return false;
                }
            }
        }
        return true;
    }

    void MoveToFrontier() {
        moreToDoInPhase = false;
        foreach(GameObject army in armies) {
            if (army.GetComponent<Army>().movesLeft > 0) {
                if (!IsOnFrontier(army.GetComponent<Army>().currentNode)) {
                    MoveTowardsClosestFrontier(army);
                }
            }
        }
    }
    bool IsOnFrontier(GameObject node) {
        foreach (GameObject neighbour in node.GetComponent<Node>().neighbours) {
            if (neighbour.GetComponent<Node>().faction != faction) return true;
        }
        return false;
    }
    void MoveTowardsClosestFrontier(GameObject army) {
        Stack<GameObject> shortestPath = PathToClosestFrontierNode(army);
        if (shortestPath.Count > 0) shortestPath.Pop();
        int movesRemaining = army.GetComponent<Army>().movesLeft;
        while (movesRemaining > 0 && shortestPath.Count>0) {
            GameObject target = shortestPath.Pop();
            print("moving to frontier:" + target.name);
            GetComponent<Player>().attackNode(army, target);
            movesRemaining--;
            moreToDoInPhase = true;
        }
    }
    Stack<GameObject> PathToClosestFrontierNode(GameObject army) {
        Faction myFaction = faction;
        GameObject startingNode = army.GetComponent<Army>().currentNode;
        List<GameObject> visited = new List<GameObject>();
        Stack<GameObject> goodPath = BFSFrontierNode(startingNode, visited, myFaction);
        if (goodPath != null) return goodPath;
        return null;
    }
    Stack<GameObject> BFSFrontierNode(GameObject currentNode, List<GameObject> visited, Faction faction) {
        if (IsOnFrontier(currentNode)) {
            Stack<GameObject> goodPath = new Stack<GameObject>();
            goodPath.Push(currentNode);
            return goodPath;
        }
        if (!visited.Contains(currentNode)) visited.Add(currentNode);
        List<GameObject> neighbours = currentNode.GetComponent<Node>().neighbours;
        Stack<GameObject> bestPath = new Stack<GameObject>();
        int bestPathLength = 999;

        foreach (GameObject neighbour in neighbours) {
            if (!visited.Contains(neighbour) && neighbour.GetComponent<Node>().occupant == null) {
                Stack<GameObject> newPath = BFSFrontierNode(neighbour, visited, faction);
                if (newPath.Count < bestPathLength) {
                    bestPath = newPath;
                    bestPathLength = newPath.Count;
                }
            }
        }
        bestPath.Push(currentNode);
        return bestPath;
    }



    /*
    // ITERATIVE DEEPENING SEEEEAAAAAAAARCH
    public List<GameObject> GetPathTo(GameObject node) {
        int depth = 1;
        Faction player = faction;
        bool solved = false;
        while (solved == false) {
            List<GameObject> goodPath = new List<GameObject>();
            goodPath = ExtendPathToNeighbours(node, goodPath, depth, player);
            if (goodPath != null) return goodPath;
            depth++;
            if (depth > 8) {
                //print("we couldn't find it");
                return null;
            }
        }
        //print("end of the line");
        return null;
    }
    // Recursive path search used in GetPathTo()
    List<GameObject> ExtendPathToNeighbours(GameObject targetNode, List<GameObject> visited, int currentDepth, Faction player) {
        visited.Add(gameObject);
        if (gameObject == targetNode) {
            //print("found the one");
            return visited;
        }
        if (currentDepth > 0) {
            currentDepth--;
            for (int i = 0; i < neighbours.Count; i++) {
                GameObject neighbour = neighbours[i];
                List<GameObject> goodPath = new List<GameObject>();
                //print("let's ask a neighbour");
                // if (neighbour.GetComponent<Node>().faction == player && !visited.Contains(neighbour)) { //For only checking friendlies
                if (!visited.Contains(neighbour) && (neighbour.GetComponent<Node>().faction == player || neighbour == targetNode)) {
                    List<GameObject> deepCopyVisited = Tools.DeepCopyGameObjectList(visited);
                    //print("checking with a neighbour");
                    goodPath = neighbour.GetComponent<Node>().ExtendPathToNeighbours(targetNode, deepCopyVisited, currentDepth, player);
                    if (goodPath != null) {
                        visited.Insert(0, gameObject);
                        //print("part of the path");
                        return goodPath;
                    }
                }
            }
        }
        //print("not here");
        return null;
    }
    */




    void SpendSavedMoney() {
        //print("invested in prophets: " + investInProphets);
        if (investInProphets >= 20) {
            GameObject originNode = GetOriginNode();
            if (originNode) {
                GetComponent<Player>().BuyProphet(originNode);
                //print("build Prophet step A");
            }
        }
        foreach (GameObject army in armies) {
            GameObject armyNode = army.GetComponent<Army>().currentNode;
            if (army.GetComponent<Army>().allocatedReplenish >= GetComponent<Player>().templeBlueprints[TempleName.Protection].cost && armyNode.GetComponent<Node>().temple==null) {
                GetComponent<Player>().BuyTemple(army.GetComponent<Army>().currentNode, TempleName.Protection);
                army.GetComponent<Army>().allocatedReplenish -= GetComponent<Player>().templeBlueprints[TempleName.Protection].cost;
            }
            if (army.GetComponent<Army>().allocatedReplenish >= GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost && armyNode.GetComponent<Node>().altar == null) {
                GetComponent<Player>().BuyAltar(army.GetComponent<Army>().currentNode, AltarName.Conflict);
                army.GetComponent<Army>().allocatedReplenish -= GetComponent<Player>().altarBlueprints[AltarName.Conflict].cost;
            }
        }
        int emptyUnitSpots = 0;
        foreach(GameObject army in armies) {
            emptyUnitSpots += 8 - army.GetComponent<Army>().units.Count;
        }

    }
    GameObject GetOriginNode() {
        foreach(GameObject node in ownedNodes) {
            if (node.GetComponent<Node>().temple != null && node.GetComponent<Node>().temple.name == TempleName.Origin && node.GetComponent<Node>().occupant == null) {
                return node;
            }
        }
        return null;
    }


    public void StartTurn() {
        turnPhase = TurnPhase.DefendAgainstThreats;
        readyToExecute = true;
        unallocatedMoney = GetComponent<Player>().money;
        IncrementArmyInfo();
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
    void IncrementArmyInfo() {

        var placeholderList = new List<(GameObject,ArmyInfo)>();

        foreach (KeyValuePair<GameObject, ArmyInfo> originalInfo in enemyArmyInfo) {
            ArmyInfo newInfo = originalInfo.Value;
            newInfo.timeSinceScout++;
            var newKeyPair = (originalInfo.Key, newInfo);
            placeholderList.Add(newKeyPair);
        }
        foreach((GameObject, ArmyInfo) info in placeholderList) {
            enemyArmyInfo[info.Item1] = info.Item2;
        }

    }
    bool readyForNextPhase() {
        if (!moreToDoInPhase && Army.readyToMove && !Army.armyAttacking) return true;
        return false;
    }
    float GetExpectedPower(GameObject army) {
        return 0;
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
     * 
     *     void BuyUnits() {
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
