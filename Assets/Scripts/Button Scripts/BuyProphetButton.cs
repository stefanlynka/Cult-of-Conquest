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
        if (!costText || !costIcon || !buttonText) {
            buttonText = Tools.GetChildNamed(gameObject, "Buy Prophet Button Text");
            costText = Tools.GetChildNamed(gameObject, "Cost Text");
            costIcon = Tools.GetChildNamed(gameObject, "Cost Icon");
        }
        if (costText) costText.SetActive(hasTempleOfOrigin);
        if (costIcon) costIcon.SetActive(hasTempleOfOrigin);
        if (hasTempleOfOrigin && buttonText) {
            buttonText.GetComponent<TextMesh>().text = "Buy Army\n";
        }
        else if (buttonText){
            buttonText.GetComponent<TextMesh>().text = "Build Temple Of\nOrigin To Create\nNew Armies";
        }
    }

    private void OnMouseDown() {
        if (hasTempleOfOrigin) {
            if (Player.human.GetComponent<Player>().BuyProphet(NodeMenu.currentNode)) {
            }
            else Tools.CreatePopup(gameObject, "Not Enough Money", 40, Color.yellow);
        }
    }
}
