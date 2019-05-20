using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleShopSpace : MonoBehaviour{

    GameObject templeName;
    GameObject cost;
    GameObject portrait;
    GameObject description;
    GameObject nodeMenu;

    GameObject templeShopManager;

    public Temple temple;

    private void Awake() {
        initializeMembers();
    }

    private void OnMouseDown() {
        print("Temple Shop Space clicked");
        if (Player.money >= temple.cost) {
            BuyTemple();
        }
    }

    public void initializeMembers() {
        templeName = Tools.GetChildNamed(gameObject, "Name Text");
        cost = Tools.GetChildNamed(gameObject, "Cost Text");
        portrait = Tools.GetChildNamed(gameObject, "Temple Portrait");
        description = Tools.GetChildNamed(gameObject, "Description Text");
        nodeMenu = GameObject.Find("Node Menu");
        templeShopManager = GameObject.Find("Temple Buying Menu");
    }

    public void SetTemple(Temple newTemple) {
        print("setup temple shop space");
        temple = newTemple;
        templeName.GetComponent<TextMesh>().text = temple.name;
        cost.GetComponent<TextMesh>().text = temple.cost.ToString();
        templeName.GetComponent<TextMesh>().text = temple.name;
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Temples/Temple of " + temple.portrait);
        description.GetComponent<TextMesh>().text = temple.description;
    }

    public void BuyTemple() {
        Player.money -= temple.cost;
        NodeMenu.currentNode.GetComponent<Node>().BuildTemple(temple);
        nodeMenu.GetComponent<NodeMenu>().LoadTemple();
        templeShopManager.GetComponent<TempleShopManager>().ExitMenu();
        Player.menuOpen = false;
    }
}
