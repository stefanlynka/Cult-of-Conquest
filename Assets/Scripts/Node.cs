using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<GameObject> Neighbours = new List<GameObject>();
    public GameObject NeighbourUpLeft;
    public GameObject NeighbourUp;
    public GameObject NeighbourUpRight;
    public GameObject NeighbourDownLeft;
    public GameObject NeighbourDown;
    public GameObject NeighbourDownRight;

    public int moneyIncome = 1;
    public int zealIncome = 0;
    public int difficulty = 0;
    public string nodeAttribute = "";
    public List<string> tempBonus = new List<string>();
    public Race homeBase = Race.None;
    public Race owner = Race.Independent;
    public GameObject occupant;
    public bool occupied = false;
    public bool occupiable = true;
    public bool highlighted = false;

    public string effigy = "";
    public int bethel = 0;
    public Altar altar = Altar.None;



    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Inputs();
    }

    public void Highlight() {
        if (!highlighted) transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-20);
        highlighted = true;
    }
    public void Unhighlight() {
        print("unhighlighted");
        if (highlighted) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
        highlighted = false;
    }

    public void Inputs() {
        /*
        if (Input.GetMouseButtonUp(0) && !justHighlighted) {
            Unhighlight();
        }
        */
    }
    private void OnMouseDown() {
        print("clicked");
        Player.nodeClicked = gameObject;
        /*
        if (Player.selectedArmy && highlighted) {
            print("newNode");
            Player.selectedArmy.GetComponent<Army>().currentNode = gameObject;
            Player.selectedArmy.GetComponent<Army>().Deselect();
        }
        */
    }
}



public enum Altar {
    Harvest,
    Devotion,
    Conflict,
    None
}

