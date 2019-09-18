using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> armies = new List<GameObject>();
    public List<GameObject> ownedNodes = new List<GameObject>();
    public List<MapUnit> unitBlueprints = new List<MapUnit>();
    public Dictionary<TempleName, Temple> templeBlueprints = new Dictionary<TempleName, Temple>();
    public Dictionary<AltarName, Altar> altarBlueprints = new Dictionary<AltarName, Altar>();
    public List<Ritual> ritualBlueprints = new List<Ritual>();
    public List<Ritual> ritualBackup = new List<Ritual>();
    public Dictionary<Faction, int> dissections = new Dictionary<Faction, int>();
    public Dictionary<string, Upgrade> upgrades = new Dictionary<string, Upgrade>();
    public Dictionary<string, Upgrade> upgradesBackup = new Dictionary<string, Upgrade>();

    public static GameObject human;
    public static GameObject selectedArmy;
    public static GameObject nodeLeftClicked;
    public static GameObject nodeRightClicked;
    public static GameObject armyLeftClicked;
    public static GameObject armyRightClicked;
    public static GameObject nodeMenu, nodeManager;
    public static GameObject battleMenu;
    public static int menuOpen = 0;

    public FactionTraits factionTraits;
    public Faction faction;
    public int money = 20;
    public int zeal = 0;
    public bool isArmySelected = false;
    public GameObject randomPanel;

    public void Awake() {
        if (!GetComponent<AI>() && !name.Contains("Neutral")) {
            human = gameObject;
            //print("I'm spartacus " + gameObject.name);
        }
    }

    // Start is called before the first frame update
    void Start(){
        Setup();
    }

    // Update is called once per frame
    void Update(){
        if (gameObject == TurnManager.currentPlayer) {
            CheckSelected();
            if (Input.GetMouseButtonDown(1)) {
                //Tools.CreatePopup(gameObject, "another check",50, Color.blue);
            }
        }
    }


    private void LateUpdate() {
        if (gameObject == TurnManager.currentPlayer) {
            ImplementClicks();
        }
    }


    public void Startup() {
        nodeMenu = GameObject.Find("/Node Menu");
        nodeManager = GameObject.Find("/Node Manager");
        battleMenu = GameObject.Find("/Battle Menu");
        randomPanel = GameObject.Find("/Random Panel");

        //SetupFactionTraits();
        SetFaction();
        if (faction == Faction.Paratrophs) SetupDissections();
    }
    public void StartGameSetup() {
        SetupOrigin();
    }

    void Setup() {
        SetupFactionTraits();
        money = 30;
        zeal = 18;
    }

    void SetFaction() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject army = transform.GetChild(i).gameObject;
            if (!armies.Contains(army)) armies.Add(army);
            army.GetComponent<Army>().faction = faction;
            army.GetComponent<Army>().currentNode.GetComponent<Node>().faction = faction;
            if (!ownedNodes.Contains(army.GetComponent<Army>().currentNode)) ownedNodes.Add(army.GetComponent<Army>().currentNode);
        }
        foreach(GameObject node in NodeManager.nodes) {
            if (node.GetComponent<Node>().faction == faction) node.GetComponent<Node>().owner = gameObject;
        }
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            node.GetComponent<Node>().faction = faction;
            node.GetComponent<Node>().owner = gameObject;
        }
    }

    void SetupFactionTraits() {
        factionTraits = GameObject.Find("/Faction Manager").GetComponent<FactionManager>().GetFactionTraits(faction);
    }

    void SetupDissections() {
        dissections.Add(Faction.Paratrophs, 10);
        dissections.Add(Faction.Carnot, 0);
        dissections.Add(Faction.Noumenon, 0);
        dissections.Add(Faction.Samata, 0);
        dissections.Add(Faction.Unmar, 0);
        dissections.Add(Faction.Zenteel, 0);
    }

    void CheckSelected() {
        isArmySelected = false;
        for (int i = 0; i < armies.Count; i++) {
            if (armies[i] != null) {
                GameObject army = armies[i];
                if (army.GetComponent<Army>().selected) {
                    isArmySelected = true;
                    selectedArmy = army;
                }
            }
        }
        if (!isArmySelected) selectedArmy = null;

        //NodeManager.highlightFog.SetActive(isArmySelected);
    }

    bool LeftClickedOnNearbyEnemy() {
        if (armyLeftClicked && armyLeftClicked.GetComponent<Army>().currentNode.GetComponent<Node>().highlighted) {
            nodeLeftClicked = armyLeftClicked.GetComponent<Army>().currentNode;
            return true;
        }
        if (nodeLeftClicked && nodeLeftClicked.GetComponent<Node>().highlighted && isArmySelected) return true;
        return false;
    }
    bool RightClickedOnNearbyEnemy() {
        if (armyRightClicked && armyRightClicked.GetComponent<Army>().currentNode.GetComponent<Node>().highlighted) {
            nodeRightClicked = armyRightClicked.GetComponent<Army>().currentNode;
            return true;
        }
        if (nodeRightClicked && nodeRightClicked.GetComponent<Node>().highlighted && isArmySelected) return true;
        return false;
    }
    bool ClickingOnAnArmy() {
        if (!selectedArmy && armyLeftClicked.GetComponent<Army>().movesLeft > 0 && armyLeftClicked.GetComponent<Army>().faction == faction) return true;
        return false;
    }
    bool ClickingOnDifferentArmy() {
        if (armyLeftClicked != selectedArmy && armyLeftClicked.GetComponent<Army>().movesLeft > 0) return true;
        return false;
    }
    bool LeftClickedAnythingElse() {
        if (selectedArmy && Input.GetMouseButtonDown(0)) return true;
        return false;
    }

    public void ImplementClicks() {

        if (menuOpen == 0) {
            if (LeftClickedOnNearbyEnemy()) {
                attackNode(selectedArmy, nodeLeftClicked);
                selectedArmy.GetComponent<Army>().Deselect();
            }
            else if (armyLeftClicked) {
                if (ClickingOnAnArmy()) {
                    armyLeftClicked.GetComponent<Army>().Select();
                }
                else if (selectedArmy) {
                    selectedArmy.GetComponent<Army>().Deselect();
                    if (ClickingOnDifferentArmy()) {
                        armyLeftClicked.GetComponent<Army>().Select();
                        selectedArmy = armyLeftClicked;
                    }
                }
            }
        
            else if (LeftClickedAnythingElse()) {
                selectedArmy.GetComponent<Army>().Deselect();
            }

            if (!selectedArmy) {
                if (nodeRightClicked) nodeMenu.GetComponent<NodeMenu>().EnterMenu(nodeRightClicked);
                else if (armyRightClicked) {
                    //print("right clicked army");
                    nodeMenu.GetComponent<NodeMenu>().EnterMenu(armyRightClicked.GetComponent<Army>().currentNode);
                }
            }
            else {
                if (randomPanel.activeSelf && RightClickedOnNearbyEnemy()) {
                    if (nodeRightClicked) randomPanel.GetComponent<RandomPanel>().AddTarget(nodeRightClicked);
                    else if (armyRightClicked) randomPanel.GetComponent<RandomPanel>().AddTarget(armyRightClicked.GetComponent<Army>().currentNode);
                    
                }
                //Add rightClickedNode to Randomizer
            }
        }
        else if (menuOpen == 1 && Input.GetMouseButtonDown(1) && NodeMenu.nodeMenuOpen == true) {
            nodeMenu.GetComponent<NodeMenu>().ExitMenu();
        }

        armyLeftClicked = null;
        armyRightClicked = null;
        nodeLeftClicked = null;
        nodeRightClicked = null;
    }

    public void attackNode(GameObject army, GameObject node) {
        print("Army: " + army.name + " aims at: " + node.name);
        //print("This cost a move");
        List<GameObject> moveList = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetPathTo(node);
        moveList.RemoveAt(0);
        for (int i = 0; i< moveList.Count; i++) {
            army.GetComponent<Army>().OrderToEnterNode(moveList[i]);
            army.GetComponent<Army>().movesLeft--;
        }
        /*
        GameObject finalNode = node;
        // If army isn't next to target, attack one step closer until adjacent
        if (!army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours.Contains(node) && army.GetComponent<Army>().currentNode != node) {
            print("extra attacks necessary");
            //print("not adjacent to target yet");
            List<GameObject> moveList = army.GetComponent<Army>().currentNode.GetComponent<Node>().GetPathTo(node);
            moveList.RemoveAt(0);
            node = moveList[0];
            attackNode(army, node);
        }
        // If army is next to target, go there        
        if (army.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours.Contains(finalNode)) {
            //print("adjacent to target");
            army.GetComponent<Army>().OrderToEnterNode(finalNode);
            //army.GetComponent<Army>().EnterNode(finalNode);
            //army.GetComponent<Army>().movesLeft -= 1;
        }
        */
    }


    public void Invade(GameObject attackingArmy, GameObject defendingNode) {
        GameObject attackArmyMenu = Tools.GetChildNamed(battleMenu, "Attacking Army Menu");
        GameObject defendArmyMenu = Tools.GetChildNamed(battleMenu, "Defending Army Menu");
        attackingArmy.GetComponent<MoveAnimator>().SetTarget(defendingNode.transform.position, true);

        if (defendingNode.GetComponent<Node>().occupant) {
            GameObject defendingArmy = defendingNode.GetComponent<Node>().occupant;
            if (defendingArmy.GetComponent<Army>().owner && defendingArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades.ContainsKey("Defensive Discord")) {
                int randInt = Random.Range(1, 11); //1 to 10
                if (randInt <= defendingArmy.GetComponent<Army>().owner.GetComponent<Player>().upgrades["Defensive Discord"].currentLevel) {
                    defendingArmy = attackingArmy.GetComponent<Army>().GetRandomDifferentTarget(defendingArmy.GetComponent<Army>().currentNode);
                }
            }
            defendArmyMenu.GetComponent<ArmyMenu>().LoadArmy(defendingArmy);
        }
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendingNode);
        battleMenu.GetComponent<BattleMenu>().SetupBattle(attackingArmy, defendingNode);
        attackArmyMenu.GetComponent<ArmyMenu>().LoadArmy(attackingArmy);
        defendArmyMenu.GetComponent<ArmyMenu>().LoadBuildings(defendingNode);
    }


    public void RemoveNode(GameObject node) {
        ownedNodes.Remove(node);
    }
    public void AddNode(GameObject node) {
        ownedNodes.Add(node);
    }
    public bool BuyRitual(Ritual ritual) {
        if (zeal >= ritual.zealCost) {
            print("Ritual Purchased");
            zeal -= ritual.zealCost;
            NodeMenu.currentNode.GetComponent<Node>().ritual = ritual.DeepCopy();
            print("ritual name = " + NodeMenu.currentNode.GetComponent<Node>().ritual.name);
            return true;
        }
        return false;
    }
    public bool BuyUpgrade(Upgrade upgrade) {
        //print("upgrade name " + upgrade.name);
        //print("upgrade level " + upgrade.currentLevel);
        //print("upgrade max " + upgrade.maxLevel);
        if (zeal >= upgrade.zealCost && upgrades.ContainsKey(upgrade.name) && upgrade.currentLevel < upgrade.maxLevel) {
            zeal -= upgrade.zealCost;
            upgrades[upgrade.name].currentLevel++;
            return true;
        }
        return false;
    }
    public bool BuyProphet(GameObject node) {
        if (money>= 20) {
            money -= 20;
            GameObject newArmy = new GameObject();
            newArmy.AddComponent<SpriteRenderer>();
            newArmy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Factions/" + faction.ToString() + "/Prophets/Prophet");
            newArmy.AddComponent<Army>();
            newArmy.AddComponent<CircleCollider2D>();
            newArmy.AddComponent<MoveAnimator>();
            newArmy.GetComponent<MoveAnimator>().SetTarget(newArmy.transform.position, false);
            newArmy.transform.position = new Vector3(node.transform.position.x, node.transform.position.y, -1);
            newArmy.transform.parent = gameObject.transform;
            newArmy.GetComponent<Army>().currentNode = node;
            newArmy.GetComponent<Army>().owner = gameObject;
            newArmy.GetComponent<Army>().faction = faction;
            newArmy.name = "Army";
            newArmy.GetComponent<Army>().AddUnit(0, true, unitBlueprints[2]);
            newArmy.transform.localScale = new Vector3(0.075f, 0.075f, 1f);
            armies.Add(newArmy);
            node.GetComponent<Node>().occupied = true;
            node.GetComponent<Node>().occupant = newArmy;
            NodeMenu.currentArmy = newArmy;
            nodeMenu.GetComponent<NodeMenu>().ProphetMenuCheck();
            return true;
        }
        return false;
    }
    public bool BuyTemple(GameObject node, TempleName templeName) {
        if (node && templeBlueprints.ContainsKey(templeName) && money >= templeBlueprints[templeName].cost) {
            node.GetComponent<Node>().BuildTemple(templeBlueprints[templeName]);
            money -= templeBlueprints[templeName].cost;
            return true;
        }
        return false;
    }
    public bool BuyAltar(GameObject node, AltarName altarName) {
        if (altarBlueprints.ContainsKey(altarName) && money >= altarBlueprints[altarName].cost) {
            node.GetComponent<Node>().BuildAltar(altarBlueprints[altarName]);
            money -= altarBlueprints[altarName].cost;
            return true;
        }
        return false;
    }

    public void StartTurn() {
        print("Start Turn!");
        money += GetMoneyIncome();
        print("Money: " + money);
        zeal += GetZealIncome();
        RestUnits();
        UpdateNodes();
        factionTraits.StartTurn(gameObject);
        DisplayFog();
        UpdateEffigies();
        if (GetComponent<AI>()) GetComponent<AI>().StartTurn();
        //print("Number 6 = " + factionTraits.UnitFunction(3));
    }

    void UpdateNodes() {
        for(int i = 0; i< ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            node.GetComponent<Node>().UpdateRitual();
        }
    }

    public int GetMoneyIncome() {
        int income = 0;
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            income += node.GetComponent<Node>().GetNodeMoneyIncome();
        }
        return income;
    }

    public int GetZealIncome() {
        int income = 0;
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            income += node.GetComponent<Node>().GetNodeZealIncome();
        }
        return income;
    }

    public int GetNodeCount() {
        return ownedNodes.Count;
    }

    private void RestUnits() {
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            army.GetComponent<Army>().Refresh();
            for (int j = 0; j < army.GetComponent<Army>().units.Count; j++) {
                if (army.GetComponent<Army>().units[j] != null) {
                    MapUnit unit = army.GetComponent<Army>().units[j];
                    unit.Refresh();
                }
            }
            
        }
    }

    public void HideUnit(MapUnit unit) {
        unit.visiblePower = 0;
        unit.hidden = true;
        money -= Mathf.RoundToInt(unit.moneyCost / 4);
    }

    public void SelectArmy(GameObject army) {
        Player.selectedArmy = army;
    }

    public void AddTarget(GameObject node) {
        GameObject randomPanel = GameObject.Find("/Random Panel");
        randomPanel.GetComponent<RandomPanel>().AddTarget(node);
    }
    public void AttackRandomTargets() {
        GameObject randomPanel = GameObject.Find("/Random Panel");
        randomPanel.GetComponent<RandomPanel>().Attack();
    }
    public void DisplayFog() {
        nodeManager.GetComponent<NodeManager>().SetNodesToHidden();
        for(int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            node.GetComponent<Node>().RevealNode(node.GetComponent<Node>().sight);
        }
        for(int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            army.GetComponent<Army>().currentNode.GetComponent<Node>().RevealNode(army.GetComponent<Army>().sight);
        }
        nodeManager.GetComponent<NodeManager>().HideStillHiddenNodes();
    }

    public void UpdateEffigies() {
        int effigyCount = 0;
        for (int i = 0; i < armies.Count; i++) {
            GameObject army = armies[i];
            if (army.GetComponent<Army>().effigy != null) effigyCount++;
        }
        for (int i = 0; i < ownedNodes.Count; i++) {
            GameObject node = ownedNodes[i];
            if (node.GetComponent<Node>().effigy != null) effigyCount++;
        }
        if (effigyCount >= 3) {
            int randInt = Random.Range(0, ownedNodes.Count);
            while (ownedNodes[randInt].GetComponent<Node>().occupied) randInt = Random.Range(0, ownedNodes.Count);
            GameObject node = ownedNodes[randInt];
            BuyProphet(node);
            GameObject godArmy = node.GetComponent<Node>().occupant;
            godArmy.GetComponent<Army>().frontRow[0] = CreateGodUnit();
        }
    }

    public MapUnit CreateGodUnit() {
        MapUnit unit = new MapUnit("God", faction, "Prelate");
        unit.maxDamage = 200;
        unit.maxHealth = 10000;
        unit.power = 1;
        unit.attackSpeed = 30;
    
        return unit;
    }

    public List<Faction> GetDissectedFactions() {
        List<Faction> factions = new List<Faction>();
        foreach(KeyValuePair<Faction, int> entry in dissections) {
            if (entry.Value >= 10) factions.Add(entry.Key);
        }
        return factions;
    }
    public void SetupOrigin() {
        foreach(GameObject node in ownedNodes) {
            if (node.GetComponent<Node>().homeBase == faction) {
                node.GetComponent<Node>().BuildTemple(templeBlueprints[TempleName.Origin]);
                node.GetComponent<Node>().BuildAltar(altarBlueprints[AltarName.Harvest]);
            }
        }
    }
}
