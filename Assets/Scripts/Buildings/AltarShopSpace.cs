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

    private void OnMouseDown() {
        if (Player.human.GetComponent<Player>().money >= altar.cost) {
            BuyAltar();
        }
        else Tools.CreatePopup(gameObject, "Not Enough Money", 40, Color.yellow);
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
        altar = newAltar;
        altarName.GetComponent<TextMesh>().text = altar.name.ToString();
        cost.GetComponent<TextMesh>().text = altar.cost.ToString();
        altarName.GetComponent<TextMesh>().text = altar.name.ToString();
        portrait.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Altars/Altar of "+altar.portrait);
        description.GetComponent<TextMesh>().text = altar.description;
    }

    public void BuyAltar() {
        Player.human.GetComponent<Player>().BuyAltar(NodeMenu.currentNode, altar.name);
        nodeMenu.GetComponent<NodeMenu>().LoadAltar();
        altarShopManager.GetComponent<AltarShopManager>().ExitMenu();
        Player.menuOpen = 1;
    }
}
