using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour {

    List<GameObject> players = new List<GameObject>();
    public static bool ritualSelected = false;
    public Ritual selectedRitual, previousRitual;
    public List<GameObject> selectedNodes;
    GameObject nodeManager;
    GameObject ritualNode;

    // Start is called before the first frame update
    void Start() {
        nodeManager = GameObject.Find("/Node Manager");
    }

    // Update is called once per frame
    void Update() {
        if (ritualSelected && Input.GetMouseButtonDown(1)) LeaveRitualSelection();
    }

    public void Startup() {
        InitializeMembers();
    }

    public void InitializeMembers() {
        GameObject playerList = GameObject.Find("/Players");
        for(int i =0; i < playerList.transform.childCount; i++) {
            GameObject child = playerList.transform.GetChild(i).gameObject;
            if (child.GetComponent<Player>()) players.Add(child);
        }
        SetupRituals();
    }


    public void SetupRitualSelection(Ritual newRitual, GameObject newNode) {
        print("Setting up Ritual Selection");
        if (newRitual.name == "Prior Ritual") newRitual = previousRitual;
        Player.menuOpen = 1;
        selectedNodes.Clear();
        ritualSelected = true;
        selectedRitual = newRitual;
        ritualNode = newNode;
        HighLightNodes();
    }
    public void SelectNode(GameObject node) {
        if (selectedNodes.Contains(node)) {
            selectedNodes.Remove(node);
            print("Already Selected, Deselecting");
        }
        else {
            selectedNodes.Add(node);
            print("Selecting");
            if (selectedNodes.Count >= selectedRitual.numTargets) {
                print("ritual name " + selectedRitual.name);
                print("ritual faction " + Tools.GetRitualFaction(selectedRitual));
                Tools.CreatePopup(node, "Ritual Cast: " + selectedRitual.name, 40, Tools.FactionToColor(Tools.GetRitualFaction(selectedRitual)));
                selectedRitual.Activate(selectedNodes);
                ritualNode.GetComponent<Node>().ritual.Clear();
                LeaveRitualSelection();
            }
        }
    }
    public void HighLightNodes() {
        nodeManager.GetComponent<NodeManager>().HighlightNodes(ritualNode, selectedRitual.range);
    }
    public void LeaveRitualSelection() {
        print("Leaving Ritual Selection");
        previousRitual = selectedRitual.DeepCopy();
        Player.menuOpen = 0;
        selectedNodes.Clear();
        selectedRitual.Clear();
        ritualSelected = false;
        nodeManager.GetComponent<NodeManager>().UnhighlightNodes();
    }


    void SetupRituals() {
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            switch (player.GetComponent<Player>().faction) {
                case Faction.Noumenon:
                    NoumenonRituals(player);
                    break;
                case Faction.Dukkha:
                    DukkhaRituals(player);
                    break;
                case Faction.Paratrophs:
                    ParatrophRituals(player);
                    break;
                case Faction.Unmar:
                    UnmarRituals(player);
                    break;
                case Faction.Samata:
                    SamataRituals(player);
                    break;
                case Faction.Carnot:
                    CarnotRituals(player);
                    break;
            }
        }
        //GameObject ritualMenu = GameObject.Find("/Ritual Menu");
        LoadPlayerRituals();

    }

    public void LoadPlayerRituals() {
        for (int j = 0; j < Player.human.GetComponent<Player>().ritualBlueprints.Count; j++) {
            GameObject ritualSlot = Tools.GetChildNamed(gameObject, "Ritual Slot " + j.ToString());
            if (ritualSlot != null) {
                Ritual blueprint = Player.human.GetComponent<Player>().ritualBlueprints[j];
                ritualSlot.GetComponent<RitualSlot>().ritualBlueprint = blueprint;
                Tools.GetChildNamed(ritualSlot, "Ritual Name Text").GetComponent<TextMesh>().text = "Ritual of " + blueprint.name;
                Tools.GetChildNamed(ritualSlot, "Ritual Zeal Cost Text").GetComponent<TextMesh>().text = blueprint.zealCost.ToString();
                Tools.GetChildNamed(ritualSlot, "Ritual Time Cost Text").GetComponent<TextMesh>().text = blueprint.prepTime.ToString();
                Tools.GetChildNamed(ritualSlot, "Ritual Description Text").GetComponent<TextMesh>().text = blueprint.description;
            }
        }
    }

    void NoumenonRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Hide Area", 2, 1, "Hide target\nnode and\nits neighbours", 2, 1, HideArea);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Hide Map", 5, 3, "Cover the\nmap in\nfog of war", 1, 1, HideMap);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void DukkhaRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "\n \n", 2, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void ParatrophRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Prior Ritual", 3, 2, "Use the\nlast ritual\nused", 6, 1, PriorRitual);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Borrow Rituals", 2, 1, "Buy rituals from\nneighbour faction\nfor one round", 2, 1, BorrowRituals);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void UnmarRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Sacrifice", 1, 2, "Sacrifice all\nmar units\nfor zeal", 5, 1, Sacrifice);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Reward Purity", 6, 4, "Buff target\narmy with only\nUnmar units", 1, 1, RewardPurity);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void SamataRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Balance Health", 3, 2, "Average health of\narmies adjacent\nto target", 3, 1, BalanceHealth);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Perfect Match", 8, 4, "Army1 becomes\na copy of\nArmy2", 5, 2, PerfectMatch);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void CarnotRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Random Step", 2, 1, "Target army\ntakes one\nrandom move", 6, 1, RandomStep);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Random Warp", 6, 2, "Warp target\narmy to a\nrandom location", 2, 1, RandomWarp);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }

    void DealDamage(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {
            for (int i = 0; i < targetArmy.GetComponent<Army>().units.Count; i++) {
                targetArmy.GetComponent<Army>().units[i].currentHealth /= 2;
            }
        }
        MakeAnimation(targetArmy);
    }
    void HideArea(List<GameObject> targets) {
        
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        GameObject targetNode = targets[0];
        targetNode.GetComponent<Node>().concealment = 5;
        for (int i = 0; i < targetNode.GetComponent<Node>().neighbours.Count; i++) {
            targetNode.GetComponent<Node>().neighbours[i].GetComponent<Node>().concealment = 5;
        }
        TurnManager.currentPlayer.GetComponent<Player>().DisplayFog();
        MakeAnimation(targetNode);
    }
    void HideMap(List<GameObject> targets) {
        List<GameObject> nodes = NodeManager.nodes;
        for (int i = 0; i< nodes.Count; i++) {
            nodes[i].GetComponent<Node>().concealment = 5;
        }
        TurnManager.currentPlayer.GetComponent<Player>().DisplayFog();
    }
    void PriorRitual(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {

        }
    }
    void BorrowRituals(List<GameObject> targets) {
        if (targets[0].GetComponent<Node>().owner != null) {
            GameObject neighbouringPlayer = targets[0].GetComponent<Node>().owner;//occupant.GetComponent<Army>().owner;
            GameObject ritualUser = TurnManager.currentPlayer;
            ritualUser.GetComponent<Player>().ritualBackup = Tools.DeepCopyRitualList(ritualUser.GetComponent<Player>().ritualBlueprints);
            ritualUser.GetComponent<Player>().ritualBlueprints = Tools.DeepCopyRitualList(neighbouringPlayer.GetComponent<Player>().ritualBlueprints);
            LoadPlayerRituals();
        }
    }
    void Sacrifice(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {
            for (int i = 0; i < targetArmy.GetComponent<Army>().units.Count; i++) {
                MapUnit unit = targetArmy.GetComponent<Army>().units[i];
                if (unit.marred) {
                    targetArmy.GetComponent<Army>().owner.GetComponent<Player>().zeal++;
                    targetArmy.GetComponent<Army>().units[i] = null;
                }
            }
            MakeAnimation(targetArmy);
        }
    }
    void RewardPurity(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null && targetArmy.GetComponent<Army>().IsPure()) {
            for (int i = 0; i < targetArmy.GetComponent<Army>().units.Count; i++) {
                MapUnit unit = targetArmy.GetComponent<Army>().units[i];
                unit.maxHealth = (int)(unit.maxHealth * 1.2);
                unit.currentHealth = unit.maxHealth;
                unit.maxDamage = (int)(unit.maxDamage * 1.2);
            }
            MakeAnimation(targetArmy);
        }
    }
    void BalanceHealth(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {
            int totalHealth = 0;
            int totalUnits = 0;
            for(int i = 0; i < targets[0].GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = targets[0].GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().occupant) {
                    for(int j = 0; j < neighbour.GetComponent<Node>().occupant.GetComponent<Army>().units.Count; j++) {
                        totalHealth += neighbour.GetComponent<Node>().occupant.GetComponent<Army>().units[j].currentHealth;
                        totalUnits++;
                    }
                }
            }
            int averageHealth = totalHealth / totalUnits;
            for (int i = 0; i < targets[0].GetComponent<Node>().neighbours.Count; i++) {
                GameObject neighbour = targets[0].GetComponent<Node>().neighbours[i];
                if (neighbour.GetComponent<Node>().occupant) {
                    for (int j = 0; j < neighbour.GetComponent<Node>().occupant.GetComponent<Army>().units.Count; j++) {
                        neighbour.GetComponent<Node>().occupant.GetComponent<Army>().units[j].TrySetHealth(averageHealth);
                    }
                }
            }
            MakeAnimation(targetArmy);
        }
    }
    void PerfectMatch(List<GameObject> targets) {
        GameObject changedArmy = targets[0].GetComponent<Node>().occupant;
        GameObject copiedArmy = targets[0].GetComponent<Node>().occupant;
        if (changedArmy != null && copiedArmy != null) {
            for(int i = 0; i < copiedArmy.GetComponent<Army>().frontRow.Length; i++) {
                MapUnit copy = copiedArmy.GetComponent<Army>().frontRow[i].DeepCopy();
                changedArmy.GetComponent<Army>().frontRow[i] = copy;
            }
            for (int i = 0; i < copiedArmy.GetComponent<Army>().backRow.Length; i++) {
                MapUnit copy = copiedArmy.GetComponent<Army>().backRow[i].DeepCopy();
                changedArmy.GetComponent<Army>().backRow[i] = copy;
            }
            MakeAnimation(copiedArmy);
            MakeAnimation(changedArmy);
        }
    }
    void RandomStep(List<GameObject> targets) {
        GameObject movingArmy = targets[0].GetComponent<Node>().occupant;
        if (movingArmy != null) {
            List<GameObject> neighbours = movingArmy.GetComponent<Army>().currentNode.GetComponent<Node>().neighbours;
            int randInt = Random.Range(0, neighbours.Count);
            movingArmy.GetComponent<Army>().NoRetreatEnterNode(neighbours[randInt]);
            MakeAnimation(movingArmy);
        }

    }
    void RandomWarp(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {
            GameObject targetNode = GameObject.Find("/Node Manager").GetComponent<NodeManager>().GetRandomNode();
            targetArmy.GetComponent<Army>().NoRetreatEnterNode(targetNode);
            MakeAnimation(targetNode);
        }
    }

    void Placeholder(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {

        }
    }
    void MakeAnimation(GameObject target) {
        GameObject animationPrefab = Resources.Load<GameObject>("Animations/Ritual/Ritual Animation");
        GameObject anim = Instantiate(animationPrefab, target.transform);
        Vector3 newPos = new Vector3(target.transform.position.x, target.transform.position.y, -20);
        anim.GetComponent<AnimationScript>().Setup(newPos, 180);
    }
}
