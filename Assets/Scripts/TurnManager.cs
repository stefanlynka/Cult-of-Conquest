using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour{

    public static GameObject human;
    public static GameObject currentPlayer;
    public static List<GameObject> players = new List<GameObject>();
    List<GameObject> nodes = new List<GameObject>();
    public int index = 0;

    Dictionary<Faction, GameObject> playerDict = new Dictionary<Faction, GameObject>();

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void InitializeMembers() {
        nodes = NodeManager.nodes;
        //players.Add(human);

        bool foundHuman = false;
        GameObject playerHolder = GameObject.Find("/Players");
        for (int i = 0; i < playerHolder.transform.childCount; i++) {
            GameObject child = playerHolder.transform.GetChild(i).gameObject;
            if (!child.GetComponent<AI>()) foundHuman = true;
            if (child.GetComponent<Player>() && foundHuman && !players.Contains(child)) players.Add(child);
        }
        for (int i = 0; i < playerHolder.transform.childCount; i++) {
            GameObject child = playerHolder.transform.GetChild(i).gameObject;
            if (!child.GetComponent<AI>()) foundHuman = true;
            if (child.GetComponent<Player>() && foundHuman && !players.Contains(child)) players.Add(child);
        }

        for (int i =0; i < players.Count; i++) {
            playerDict.Add(players[i].GetComponent<Player>().faction, players[i]);
        }
        human = Player.human;
        currentPlayer = human;
    }

    public void NextTurn() {
        print("Turn Over");
        currentPlayer.GetComponent<Player>().factionTraits.EndTurn(currentPlayer);
        index++;
        if (index >= players.Count) index = 0;
        currentPlayer = players[index];
        GameObject.Find("/Unit Buying Menu/Unit Spaces").GetComponent<UnitShopManager>().AssignUnits(currentPlayer);
        //GameObject.Find("/Unit Buying Menu/Unit Spaces").GetComponent<UnitShopManager>().MakeUnits(currentPlayer.GetComponent<Player>().faction);
        currentPlayer.GetComponent<Player>().StartTurn();
    }

}
