using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    public GameObject neighbourUpLeft;
    public GameObject neighbourUp;
    public GameObject neighbourUpRight;
    public GameObject neighbourDownLeft;
    public GameObject neighbourDown;
    public GameObject neighbourDownRight;

    public GameObject nodeMenu;

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
        nodeMenu = GameObject.Find("/Node Menu");
    }

    // Update is called once per frame
    void Update(){
        Inputs();
    }

    public void Highlight() {
        if (!highlighted) transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-10);
        highlighted = true;
    }
    public void Unhighlight() {
        if (highlighted) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
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
        Player.nodeClicked = gameObject;
    }
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            //nodeMenu.GetComponent<NodeMenu>().EnterMenu(gameObject);
            Player.nodeClicked = gameObject;
        }
    }
    public GameObject GetOccupant() {
        if (occupant) return occupant;
        else return null;
    }
}



public enum Altar {
    Harvest,
    Devotion,
    Conflict,
    None
}

