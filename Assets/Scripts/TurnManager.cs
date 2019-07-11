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

        GameObject playerHolder = GameObject.Find("/Players");
        for (int i = 0; i < playerHolder.transform.childCount; i++) {
            GameObject child = playerHolder.transform.GetChild(i).gameObject;
            if (child != human) players.Add(child);
        }

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
        GameObject.Find("/Unit Buying Menu/Unit Spaces").GetComponent<UnitShopManager>().AssignUnits(currentPlayer);
        //GameObject.Find("/Unit Buying Menu/Unit Spaces").GetComponent<UnitShopManager>().MakeUnits(currentPlayer.GetComponent<Player>().race);
        currentPlayer.GetComponent<Player>().StartTurn();
    }

}
