using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour{

    public static GameObject human;
    public static GameObject currentPlayer;
    List<GameObject> players = new List<GameObject>();
    List<GameObject> nodes = new List<GameObject>();
    public int index = 0;

    Dictionary<Race, GameObject> playerDict = new Dictionary<Race, GameObject>();

    private void Awake() {
        human = GameObject.Find("/Players/Human");
    }

    // Start is called before the first frame update
    void Start(){
        InitializeMembers();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void InitializeMembers() {
        nodes = NodeManager.nodes;
        players.Add(human);
        for(int i =0; i < players.Count; i++) {
            playerDict.Add(players[i].GetComponent<Player>().race, players[i]);
        }
        currentPlayer = human;
    }

    public void NextTurn() {
        print("Next Turn");
        index++;
        if (index >= players.Count) index = 0;
        currentPlayer = players[index];
        currentPlayer.GetComponent<Player>().StartTurn();
    }

}
