using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleShopSpace : MonoBehaviour{

    GameObject templeName;
    GameObject cost;
    GameObject portrait;
    GameObject description;
    GameObject nodeMenu;
    GameObject human;

    GameObject templeShopManager;

    public Temple temple;

    private void Awake() {
        initializeMembers();
    }

    private void OnMouseDown() {
        if (human.GetComponent<Player>().money >= temple.cost) {
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
        human = Player.human;
    }

    public void SetTemple(Temple newTemple) {
        temple = newTemple;
        templeName.GetComponent<TextMesh>().text = temple.name.ToString();
        cost.GetComponent<TextMesh>().text = temple.cost.ToString();
        templeName.GetComponent<TextMesh>().text = temple.name.ToString();
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Temples/Temple of " + temple.portrait);
        description.GetComponent<TextMesh>().text = temple.description;
    }

    public void BuyTemple() {
        human.GetComponent<Player>().money -= temple.cost;
        NodeMenu.currentNode.GetComponent<Node>().BuildTemple(temple);
        nodeMenu.GetComponent<NodeMenu>().LoadTemple();
        templeShopManager.GetComponent<TempleShopManager>().ExitMenu();
        Player.menuOpen = 1;
    }
}
