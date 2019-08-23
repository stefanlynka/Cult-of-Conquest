using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour {

    List<GameObject> players = new List<GameObject>();
    GameObject human;
    public static bool ritualSelected = false;
    public Ritual selectedRitual;
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
        human = Player.human;
        SetupRituals();
    }


    public void SetupRitualSelection(Ritual newRitual, GameObject newNode) {
        print("Setting up Ritual Selection");
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
        for (int j = 0; j < human.GetComponent<Player>().ritualBlueprints.Count; j++) {
            GameObject ritualSlot = Tools.GetChildNamed(gameObject, "Ritual Slot " + j.ToString());
            print(ritualSlot.name);
            if (ritualSlot != null){
                print("found the slot");
                Ritual blueprint = human.GetComponent<Player>().ritualBlueprints[j];
                ritualSlot.GetComponent<RitualSlot>().ritualBlueprint = blueprint;
                Tools.GetChildNamed(ritualSlot, "Ritual Name Text").GetComponent<TextMesh>().text = "Ritual of " + blueprint.name;
                Tools.GetChildNamed(ritualSlot, "Ritual Zeal Cost Text").GetComponent<TextMesh>().text = blueprint.zealCost.ToString();
                Tools.GetChildNamed(ritualSlot, "Ritual Time Cost Text").GetComponent<TextMesh>().text = blueprint.prepTime.ToString();
                Tools.GetChildNamed(ritualSlot, "Ritual Description Text").GetComponent<TextMesh>().text = blueprint.description;
            }
        }

    }
    void NoumenonRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void DukkhaRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void ParatrophRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void UnmarRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void SamataRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void CarnotRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round", 3, 1, DealDamage);
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }

    void DealDamage(List<GameObject> targets) {
        GameObject targetArmy = targets[0].GetComponent<Node>().occupant;
        if (targetArmy != null) {
            for (int i = 0; i < targetArmy.GetComponent<Army>().units.Length; i++) {
                MapUnit unit = targetArmy.GetComponent<Army>().units[i];
                if (unit != null) unit.currentHealth /= 2;
            }
        }
    }
}
