using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour{

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
            GameObject player = playerHolder.transform.GetChild(i).gameObject;
            if (player == Player.human) foundHuman = true;
            if (player.GetComponent<Player>() && foundHuman && !players.Contains(player)) players.Add(player);
        }
        for (int i = 0; i < playerHolder.transform.childCount; i++) {
            GameObject player = playerHolder.transform.GetChild(i).gameObject;
            if (player.GetComponent<Player>() && foundHuman && !players.Contains(player)) players.Add(player);
        }

        for (int i =0; i < players.Count; i++) {
            playerDict.Add(players[i].GetComponent<Player>().faction, players[i]);
        }
        currentPlayer = Player.human;
        for(int i=0; i< players.Count; i++) {
            print("Player: " + players[i].name);
        }
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
        Army.readyToMove = true;
    }

}
