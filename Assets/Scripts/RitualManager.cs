using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour {

    List<GameObject> players = new List<GameObject>();
    GameObject human;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Startup() {
        InitializeMembers();
    }

    public void InitializeMembers() {
        print("rituals starting");
        GameObject playerList = GameObject.Find("/Players");
        for (int i = 0; i < playerList.transform.childCount; i++) {
            GameObject child = playerList.transform.GetChild(i).gameObject;
            if (child.GetComponent<Player>()) {
                players.Add(child);
                if (!child.GetComponent<AI>()) human = child;
            }
        }
        SetupRituals();
    }

    void SetupRituals() {
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            switch (player.GetComponent<Player>().race) {
                case Race.Noumenon:
                    NoumenonRituals(player);
                    break;
                case Race.Dukkha:
                    DukkhaRituals(player);
                    break;
                case Race.Paratrophs:
                    ParatrophRituals(player);
                    break;
                case Race.Unmar:
                    UnmarRituals(player);
                    break;
                case Race.Eidalons:
                    EidalonRituals(player);
                    break;
                case Race.Carnot:
                    CarnotRituals(player);
                    break;
            }
        }
        GameObject ritualMenu = GameObject.Find("/Ritual Menu");
        for (int j = 0; j < human.GetComponent<Player>().ritualBlueprints.Count; j++) {
            GameObject ritualSlot = Tools.GetChildNamed(ritualMenu, "Ritual Slot " + j.ToString());
            if (ritualSlot != null){
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
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void DukkhaRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void ParatrophRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void UnmarRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void EidalonRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
    void CarnotRituals(GameObject player) {
        Ritual ritual1 = new Ritual("Restoration", 3, 2, "Heal all\nunits to\nfull HP");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual1);
        Ritual ritual2 = new Ritual("Devastation", 4, 7, "Double unit\ndamage for\none round");
        player.GetComponent<Player>().ritualBlueprints.Add(ritual2);
    }
}
