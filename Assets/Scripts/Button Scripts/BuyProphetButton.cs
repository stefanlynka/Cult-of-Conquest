using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyProphetButton : MonoBehaviour{

    GameObject buttonText, costText, costIcon;
    bool hasTempleOfOrigin;

    // Start is called before the first frame update
    void Start(){
        buttonText = Tools.GetChildNamed(gameObject, "Buy Prophet Button Text");
        costText = Tools.GetChildNamed(gameObject, "Cost Text");
        costIcon = Tools.GetChildNamed(gameObject, "Cost Icon");
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SetTextByTemple(bool templeOfOriginPresent) {
        hasTempleOfOrigin = templeOfOriginPresent;
        costText.SetActive(hasTempleOfOrigin);
        costIcon.SetActive(hasTempleOfOrigin);
        if (hasTempleOfOrigin) {
            buttonText.GetComponent<TextMesh>().text = "Buy Army\n";
        }
        else {
            buttonText.GetComponent<TextMesh>().text = "Build Temple Of\nOrigin To Create\nNew Armies";
        }
    }

    private void OnMouseDown() {
        if (hasTempleOfOrigin) {
            if (Player.human.GetComponent<Player>().BuyProphet(NodeMenu.currentNode)) {
            }
        }
    }
}
