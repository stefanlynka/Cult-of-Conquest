using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarShopSpace : MonoBehaviour{

    GameObject altarName;
    GameObject cost;
    GameObject portrait;
    GameObject description;
    GameObject nodeMenu;
    GameObject human;

    GameObject altarShopManager;

    public Altar altar;

    private void Awake() {
        initializeMembers();
    }

    private void OnMouseDown() {
        if (human.GetComponent<Player>().money >= altar.cost) {
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
        human = Player.human;
    }

    public void SetAltar(Altar newAltar) {
        altar = newAltar;
        altarName.GetComponent<TextMesh>().text = altar.name.ToString();
        cost.GetComponent<TextMesh>().text = altar.cost.ToString();
        altarName.GetComponent<TextMesh>().text = altar.name.ToString();
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Altars/Altar of "+altar.portrait);
        description.GetComponent<TextMesh>().text = altar.description;
    }

    public void BuyAltar() {
        human.GetComponent<Player>().money -= altar.cost;
        NodeMenu.currentNode.GetComponent<Node>().BuildAltar(altar);
        nodeMenu.GetComponent<NodeMenu>().LoadAltar();
        altarShopManager.GetComponent<AltarShopManager>().ExitMenu();
        Player.menuOpen = 1;
    }
}
