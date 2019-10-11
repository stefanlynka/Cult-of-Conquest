using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {
    public bool selected = false;
    public static GameObject highlightFog;
    public static bool readyToMove = true;
    public static List<Intent> movesToDo = new List<Intent>();

    public Faction faction;
    public GameObject currentNode;
    public GameObject nodeManager;
    public GameObject owner;
    public bool mouseOverArmy = false;
    public int maxMoves = 2;
    public int movesLeft = 2;
    public int sight = 2;
    public bool marredBattle = false;
    public float enemyPower = 0;

    public List<MapUnit> units = new List<MapUnit>();
    public MapUnit[] backRow = new MapUnit[4];
    public MapUnit[] frontRow = new MapUnit[4];
    public List<MapUnit> defeatedEnemies = new List<MapUnit>();
    public Dictionary<string, int> precombatUnits = new Dictionary<string, int>();
    public Temple conqueredTemple;
    public Altar conqueredAltar;
    public float precombatPower;
    public Effigy effigy;
    public int allocatedMoney = 0;
    public int allocatedReplenish = 0;
    public List<MapUnit> unitsToAdd = new List<MapUnit>();

    // Start is called before the first frame update
    void Start() {
        nodeManager = GameObject.Find("/Node Manager");
        ResetPrecombatUnits();
    }

    // Update is called once per frame
    void Update() {
        MakeMoves();
        //if (owner == TurnManager.currentPlayer) print("Moves left: " + movesLeft);
    }

    public void Startup() {
        SetStartingUnits();
    }

    private void OnMouseEnter() {
        mouseOverArmy = true;
    }

    private void OnMouseExit() {
        mouseOverArmy = false;
    }
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Player.armyLeftClicked = gameObject;
        }
        if (Input.GetMouseButtonDown(1) && TurnManager.currentPlayer.GetComponent<Player>().faction == faction) {
            Player.armyRightClicked = gameObject;
        }
    }
    private void OnMouseDown() {
        if (RitualManager.ritualSelected) {
            if (RitualManager.ritualSelected && currentNode.GetComponent<Node>().highlighted) GameObject.Find("/Ritual Menu").GetComponent<RitualManager>().SelectNode(currentNode);
        }
    }

    void MakeMoves() {
        if (movesToDo.Count > 0 && readyToMove) {
            float randFloat = Random.Range(0, 10000);
            Intent move = movesToDo[0];
            movesToDo.Remove(move);
            print("New Move: About to switch readyToMove from:" + readyToMove + " " + randFloat.ToString());
            readyToMove = false;
            print("New Move: Just switched readyToMove to:" + readyToMove + " " + randFloat.ToString());
            print("Army: " + move.armyMoving.name + " moving to: " + move.targetNode + " " + randFloat.ToString());
            if (move.armyMoving) move.armyMoving.GetComponent<Army>().EnterNode(move.targetNode);
            else readyToMove = true;
        }
    }

    void SetStartingUnits() {
        owner = Player.human;
        GameObject playerList = GameObject.Find("/Players");
        owner = transform.parent.gameObject;
        AddUnit(0, true, owner.GetComponent<Player>().unitBlueprints[2]);
        //AddUnit(1, true, owner.GetComponent<Player>().unitBlueprints[0]);
        //AddUnit(2, true, owner.GetComponent<Player>().unitBlueprints[2]);
        //AddUnit(3, true, owner.GetComponent<Player>().unitBlueprints[3]);
        //AddUnit(0, false, owner.GetComponent<Player>().unitBlueprints[0]);
        //AddUnit(1, false, owner.GetComponent<Player>().unitBlueprints[0]);
        //AddUnit(2, false, owner.GetComponent<Player>().unitBlueprints[0]);
        //AddUnit(3, false, owner.GetComponent<Player>().unitBlueprints[0]);
    }


    public void AddToDamageMod(float modifier) {
        for (int i = 0; i < units.Count; i++) {
            MapUnit unit = units[i];
            unit.damageMod += modifier;
            //print("new modifier = " + unit.damageMod);
        }
    }

    public void HighlightNodes(GameObject node, int distance) {
        nodeManager.GetComponent<NodeManager>().HighlightAttackableNodes(node, distance, faction);
    }

    public void UnhighlightNodes() {
        nodeManager.GetComponent<NodeManager>().UnhighlightNodes();
    }
    public void Select() {
        selected = true;
        transform.localScale *= 2;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -20);
        HighlightNodes(currentNode, movesLeft);
    }
    public void Deselect() {
        selected = false;
        transform.localScale *= 0.5f;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        UnhighlightNodes();
    }

    public void EnterNode(GameObject targetNode) {
        if (targetNode.GetComponent<Node>().occupiable) {

            if (targetNode.GetComponent<Node>().faction != faction) {
                owner.GetComponent<Player>().Invade(gameObject, targetNode);
            }
            else {
                if (targetNode.GetComponent<Node>().occupied && targetNode.GetComponent<Node>().occupant != gameObject) {
                    GameObject otherArmy = targetNode.GetComponent<Node>().occupant;
                    SwitchNodes(currentNode, otherArmy, targetNode);
                }
                else {
                    MoveToNode(targetNode);
                }
            }
        }
    }

    public void SwitchNodes(GameObject node1, GameObject army2, GameObject node2) {
        MoveToNode(node2);
        army2.GetComponent<Army>().MoveToNode(node1);
        node2.GetComponent<Node>().occupied = true;
        node2.GetComponent<Node>().occupant = gameObject;
    }

    public void NoRetreatEnterNode(GameObject targetNode) {
        BattleMenu.retreatAllowed = false;
        EnterNode(targetNode);
    }

    public void MoveToNode(GameObject destination) {
        //print("moving, attacking = false");
        GetComponent<MoveAnimator>().SetTarget(destination.transform.position, false);
        currentNode.GetComponent<Node>().occupied = false;
        currentNode.GetComponent<Node>().occupant = null;
        //transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        currentNode = destination;
        destination.GetComponent<Node>().faction = faction;
        destination.GetComponent<Node>().occupied = true;
        destination.GetComponent<Node>().occupant = gameObject;
        destination.GetComponent<Node>().owner = owner;
        destination.GetComponent<Node>().UpdateSprite();
        if (owner.GetComponent<AI>()) {
            //owner.GetComponent<AI>().readyToExecute = true;
        }
    }

    public void OrderToEnterNode(GameObject targetNode) {
        //print("Order given to attack: " + targetNode.name);
        Intent order = new Intent(gameObject, targetNode);
        movesToDo.Add(order);
    }
    public void OrderToEnterNodeNow(GameObject targetNode) {
        Intent order = new Intent(gameObject, targetNode);
        movesToDo.Insert(0, order);
    }


    public void BuyUnit(UnitPos unitPos, MapUnit unit) {
        //print("trying to buy unit");
        int unitCost = currentNode.GetComponent<Node>().GetUnitCost(Tools.UnitToIndex(unit));
        if (unitCost <= owner.GetComponent<Player>().money && unit.zealCost <= owner.GetComponent<Player>().zeal) {
            if (owner.GetComponent<Player>().upgrades.ContainsKey("Protect the Pure")) {
                float upgradeLevel = owner.GetComponent<Player>().upgrades["Protect the Pure"].currentLevel;
                unit.maxHealth = (int)(unit.maxHealth - 0.1f * upgradeLevel);
                unit.currentHealth = unit.maxHealth;
                unit.maxShield += (int)upgradeLevel * 2;
                unit.currentShield = unit.maxShield;
            }
            owner.GetComponent<Player>().money -= unitCost;
            owner.GetComponent<Player>().zeal -= unit.zealCost;
            owner.GetComponent<Player>().factionTraits.NewUnit(unit);
            AddUnit(unitPos.position, unitPos.frontRow, unit);
        }
        //else print("not enough money");
    }

    public void BuyFakeUnit(UnitPos unitPos, MapUnit unit) {
        //unit.maxDamage = 0;
        unit.moneyCost = Mathf.RoundToInt(unit.moneyCost * 0.4f);
        unit.zealCost = 0;
        unit.name += " fake";
        unit.portraitName += " fake";
        unit.fake = true;
        BuyUnit(unitPos, unit);
    }
    public void DissectUnit(MapUnit unit) {
        RemoveUnit(unit);
        owner.GetComponent<Player>().IncreaseZeal(1);
        if (owner.GetComponent<Player>().dissections.ContainsKey(unit.faction)) owner.GetComponent<Player>().dissections[unit.faction]++;
    }
    public void SellUnit(MapUnit unit) {
        owner.GetComponent<Player>().money += unit.moneyCost / 4;
        RemoveUnit(unit);
    }
    public void AddUnit(int index, bool fRow, MapUnit unit) {
        //print("adding unit");
        MapUnit newUnit = unit.DeepCopy();
        if (fRow) frontRow[index] = newUnit;
        else backRow[index] = newUnit;
        if (fRow) index += 4;
        units.Add(newUnit);
    }

    public MapUnit GetUnit(UnitPos pos) {
        if (pos.frontRow) return frontRow[pos.position];
        else return backRow[pos.position];
    }
    public void RemoveUnit(MapUnit unit) {
        units.Remove(unit);
        RemoveUnitFromArray(frontRow, unit);
        RemoveUnitFromArray(backRow, unit);
        if (units.Count == 0) {
            DeleteArmy();
        }
    }
    public void DeleteArmy() {
        currentNode.GetComponent<Node>().occupant = null;
        currentNode.GetComponent<Node>().occupied = false;
        owner.GetComponent<Player>().armies.Remove(gameObject);
        Destroy(gameObject);
    }

    void RemoveUnitFromArray(MapUnit[] array, MapUnit unit) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i] == unit) {
                array[i] = null;
            }
        }
    }

    public void Defeated() {
        print("defeated");
        //readyToMove = true;
        currentNode.GetComponent<Node>().occupant = null;
        currentNode.GetComponent<Node>().occupied = false;
        owner.GetComponent<Player>().armies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void Refresh() {
        movesLeft = maxMoves;
    }
    /*
    public int GetPower() {
        int power = 0;
        for (int i = 0; i < units.Count; i++) {
            power += units[i].power;
            //print("found some power: " + unit.power);
        }
        //power += currentNode.GetComponent<Node>().GetPower();
        return power;
    }
    */
    public float GetOffensivePower() {
        float totalHealth = 0;
        float totalDPS = 0;
        float power = 0;
        for (int i = 0; i < frontRow.Length; i++) {
            if (frontRow[i] != null) {
                totalHealth += frontRow[i].GetHealth();
                totalDPS += frontRow[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        totalDPS = 0;
        for (int i = 0; i < backRow.Length; i++) {
            if (backRow[i] != null) {
                totalHealth += backRow[i].GetHealth();
                totalDPS += backRow[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        return power;

        /*
float totalHealth = 0;
float power = 0;
for (int i = 0; i < frontRow.Length; i++) {
    if (frontRow[i] != null) {
        totalHealth += frontRow[i].GetHealth();
        power += frontRow[i].GetDPS() * totalHealth;
    }
}
for (int i = 0; i < backRow.Length; i++) {
    if (backRow[i] != null) {
        totalHealth += backRow[i].GetHealth();
        power += backRow[i].GetDPS() * totalHealth;
    }
}
//print("Army Power: " + power);
return power;
*/
    }

    public float GetDefensivePower() {
        float totalHealth = 0;
        float totalDPS = 0;
        float power = 0;
        if (currentNode.GetComponent<Node>().temple != null && currentNode.GetComponent<Node>().temple.name == TempleName.Protection) {
            totalHealth += currentNode.GetComponent<Node>().temple.unit.GetHealth();
            power += currentNode.GetComponent<Node>().temple.unit.GetDPS() * totalHealth;
        }
        if (currentNode.GetComponent<Node>().altar != null && currentNode.GetComponent<Node>().altar.name == AltarName.Conflict) {
            totalHealth += currentNode.GetComponent<Node>().altar.unit.GetHealth();
            power += currentNode.GetComponent<Node>().altar.unit.GetDPS() * totalHealth;
        }
        totalDPS = 0;
        for (int i = 0; i < frontRow.Length; i++) {
            if (frontRow[i] != null) {
                totalHealth += frontRow[i].GetHealth();
                totalDPS += frontRow[i].GetDPS();
            }
        }
        power += totalDPS * totalHealth;
        totalDPS = 0;
        for (int i = 0; i < backRow.Length; i++) {
            if (backRow[i] != null) {
                totalHealth += backRow[i].GetHealth();
                power += backRow[i].GetDPS() * totalHealth;
            }
        }
        power += totalDPS * totalHealth;
        return power;
    }


    public bool HasOpenPosition() {
        if (units.Count < 8) return true;
        print("no openings");
        return false;
    } 

    public UnitPos GetOpenPosition() {
        for (int i = 0; i < frontRow.Length; i++) {
            if (frontRow[i] == null) {
                return new UnitPos(i, true);
            }
        }
        for (int i = 0; i < backRow.Length; i++) {
            if (backRow[i] == null) {
                return new UnitPos(i, false);
            }
        }
        return new UnitPos(0, true);
    }

    public bool Isolated() {
        List<GameObject> nodes = GetConnectedNodes();
        for (int i = 0; i < nodes.Count; i++) {
            GameObject node = nodes[i];
            if (node.GetComponent<Node>().occupied && node.GetComponent<Node>().occupant != gameObject) return false;
        }

        return true;
    }
    public List<GameObject> GetConnectedNodes() {
        List<GameObject> nodes = new List<GameObject>();
        List<GameObject> frontier = new List<GameObject>();
        GameObject origin = currentNode;
        frontier.Add(origin);
        while (frontier.Count > 0) {
            GameObject currentNode = frontier[0];

            for (int i = 0; i < currentNode.GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = currentNode.GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().faction == faction && !nodes.Contains(neighbour) && !frontier.Contains(neighbour)) {
                    frontier.Add(neighbour);
                }
            }
            nodes.Add(frontier[0]);
            frontier.RemoveAt(0);
        }
        return nodes;
    }
    public void ResetArmy() {
        for (int i =0; i < units.Count; i++) {
            units[i].Reset();
        }
    }
    public void PrebattleSetup() {
        //ResetArmy();
        SetPrecombatPower();
        SetPrecombatUnits();
    }
    public void SetPrecombatPower() {
        precombatPower = GetOffensivePower();
    }
    public void SetPrecombatUnits() {
        ResetPrecombatUnits();
        foreach (MapUnit unit in units) {
            precombatUnits[unit.name]++;
        }
    }
    void ResetPrecombatUnits() {
        precombatUnits.Clear();
        precombatUnits.Add("peon", 0);
        precombatUnits.Add("acolyte", 0);
        precombatUnits.Add("shaman", 0);
        precombatUnits.Add("prelate", 0);
    }
    public bool IsPure() {
        for (int i = 0; i < units.Count; i++) {
            if(units[i].marred) return false;
        }
        return true;
    }
    public GameObject GetRandomDifferentTarget(GameObject oldNode) {
        GameObject newNode;
        int randInt = Random.Range(0, currentNode.GetComponent<Node>().neighbours.Count);
        newNode = currentNode.GetComponent<Node>().neighbours[randInt];
        if (newNode == oldNode) {
            if (randInt == currentNode.GetComponent<Node>().neighbours.Count-1) newNode = currentNode.GetComponent<Node>().neighbours[randInt-1];
            else newNode = currentNode.GetComponent<Node>().neighbours[randInt + 1];
        }
        return newNode;
    }
}


