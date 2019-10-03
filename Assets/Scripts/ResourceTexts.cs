using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTexts : MonoBehaviour
{
    GameObject moneyText;
    GameObject zealText;
    // Start is called before the first frame update
    void Start(){
        moneyText = Tools.GetChildNamed(gameObject, "Money Text");
        zealText = Tools.GetChildNamed(gameObject, "Zeal Text");
    }

    // Update is called once per frame
    void Update() {
        if (Player.human != null) {
            moneyText.GetComponent<TextMesh>().text = Player.human.GetComponent<Player>().money + " (" + Player.human.GetComponent<Player>().GetMoneyIncome() + ")";
            zealText.GetComponent<TextMesh>().text = Player.human.GetComponent<Player>().zeal + " (" + Player.human.GetComponent<Player>().GetZealIncome() + ")";
        }
    }
}
