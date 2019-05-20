using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarShopSpace : MonoBehaviour{

    GameObject altarName;
    GameObject cost;
    GameObject portrait;
    GameObject description;
    GameObject nodeMenu;

    GameObject altarShopManager;

    public Altar altar;

    private void Awake() {
        initializeMembers();
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    private void OnMouseDown() {
        print("Altar Shop Space clicked");
        if (Player.money >= altar.cost) {
            BuyAltar();
        }
    }

    public void initializeMembers() {
        altarName = Tools.GetChildNamed(gameObject, "Name Text");
        cost = Tools.GetChildNamed(gameObject, "Cost Text");
        portrait = Tools.GetChildNamed(gameObject, "Altar Portrait");
        description = Tools.GetChildNamed(gameObject, "Description Text");
        nodeMenu = GameObject.Find("Node Menu");
        altarShopManager = GameObject.Find("Altar Buying Menu");
    }
    
    public void SetAltar(Altar newAltar) {
        print("setup altar shop space");
        altar = newAltar;
        altarName.GetComponent<TextMesh>().text = altar.name;
        cost.GetComponent<TextMesh>().text = altar.cost.ToString();
        altarName.GetComponent<TextMesh>().text = altar.name;
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Altars/Altar of "+altar.portrait);
        description.GetComponent<TextMesh>().text = altar.description;
    }

    public void BuyAltar() {
        Player.money -= altar.cost;
        NodeMenu.currentNode.GetComponent<Node>().BuildAltar(altar);
        nodeMenu.GetComponent<NodeMenu>().LoadAltar();
        altarShopManager.GetComponent<AltarShopManager>().ExitMenu();
        Player.menuOpen = false;
    }
}
