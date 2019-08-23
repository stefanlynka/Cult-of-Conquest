using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public bool selected = false;
    public static GameObject highlightFog;

    public Faction faction;
    public GameObject currentNode;
    public GameObject nodeManager;
    public GameObject owner;
    public bool mouseOverArmy = false;
    public int maxMoves = 2;
    public int movesLeft = 2;
    public bool marredBattle = false;

    public MapUnit[] units = new MapUnit[8];
    public MapUnit[] backRow = new MapUnit[4];
    public MapUnit[] frontRow = new MapUnit[4];
    public List<MapUnit> defeatedEnemies = new List<MapUnit>();
    public int precombatPower;

    // Start is called before the first frame update
    void Start(){
        nodeManager = GameObject.Find("/Node Manager");
    }

    // Update is called once per frame
    void Update(){
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
        if (Input.GetMouseButtonDown(0) && TurnManager.currentPlayer.GetComponent<Player>().faction==faction) {
            Player.armyLeftClicked = gameObject;
        }
        if (Input.GetMouseButtonDown(1) && TurnManager.currentPlayer.GetComponent<Player>().faction == faction) {
            Player.armyRightClicked = gameObject;
        }
    }

    void SetStartingUnits() {
        owner = TurnManager.human;
        GameObject playerList = GameObject.Find("/Players");
        owner = transform.parent.gameObject;
        AddUnit(0, true, owner.GetComponent<Player>().unitBlueprints[2]);
    }

    public void SetPrecombatPower() {
        precombatPower = 0;
        for(int i = 0; i < units.Length; i++) {
            if (units[i] != null) precombatPower += units[i].power;
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
    public void MoveToNode(GameObject destination) {
        currentNode.GetComponent<Node>().occupied = false;
        currentNode.GetComponent<Node>().occupant = null;
        transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        currentNode = destination;
        destination.GetComponent<Node>().faction = faction;
        destination.GetComponent<Node>().occupied = true;
        destination.GetComponent<Node>().occupant = gameObject;
        destination.GetComponent<Node>().UpdateSprite();
        if (owner.GetComponent<AI>()) {
            owner.GetComponent<AI>().readyToExecute = true;
        }
    }

    public void BuyUnit(UnitPos unitPos, MapUnit unit) {
        print("trying to buy unit");
        if (unit.moneyCost <= owner.GetComponent<Player>().money && unit.zealCost <= owner.GetComponent<Player>().zeal) {
            owner.GetComponent<Player>().money -= unit.moneyCost;
            owner.GetComponent<Player>().zeal -= unit.zealCost;
            owner.GetComponent<Player>().factionTraits.NewUnit(unit);
            print("new shield = "+unit.maxShield);
            AddUnit(unitPos.position, unitPos.frontRow, unit);
        }
        else print("not enough money");
    }
    public void BuyFakeUnit(UnitPos unitPos, MapUnit unit) {
        unit.maxDamage = 0;
        unit.moneyCost = Mathf.RoundToInt(unit.moneyCost * 0.4f);
        unit.zealCost = 0;
        unit.name += "Fake";
        unit.portraitName += "Fake";
        BuyUnit(unitPos, unit);
    }
    public void DissectUnit(MapUnit unit) {
        RemoveUnit(unit);
        owner.GetComponent<Player>().zeal++;
    }

    public void AddUnit(int index, bool fRow, MapUnit unit) {
        //print("adding unit");
        MapUnit newUnit = unit.DeepCopy();
        if (fRow) frontRow[index] = newUnit;
        else backRow[index] = newUnit;
        if (fRow) index += 4;
        units[index] = newUnit;
    }

    public MapUnit GetUnit(UnitPos pos) {
        if (pos.frontRow) return frontRow[pos.position];
        else return backRow[pos.position];
    }
    public void RemoveUnit(MapUnit unit) {
        RemoveUnitFromArray(units, unit);
        RemoveUnitFromArray(frontRow, unit);
        RemoveUnitFromArray(backRow, unit);
        print("units removed");
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
        currentNode.GetComponent<Node>().occupant = null;
        currentNode.GetComponent<Node>().occupied = false;
        owner.GetComponent<Player>().armies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void Refresh() {
        movesLeft = maxMoves;
    }

    public int GetPower() {
        int power = 0;
        for(int i = 0; i < units.Length; i++) {
            if (units[i] != null) {
                power += units[i].power;
                //print("found some power: " + unit.power);
            }
        }

        return power;
    }

    public bool HasOpenPosition() {
        for (int i = 0 ; i < units.Length; i++) {
            if (units[i] == null) return true;
        }
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
        for (int i =0; i < units.Length; i++) {
            MapUnit unit = units[i];
            if (unit != null) {
                unit.Reset();
            }
        }
    }
    public void PrebattleSetup() {
        SetPrecombatPower();
        ResetArmy();
    }
}


