using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTexts : MonoBehaviour
{
    GameObject moneyText;
    GameObject zealText;
    GameObject human;
    // Start is called before the first frame update
    void Start(){
        moneyText = Tools.GetChildNamed(gameObject, "Money Text");
        zealText = Tools.GetChildNamed(gameObject, "Zeal Text");
        human = Player.human;
    }

    // Update is called once per frame
    void Update(){
        moneyText.GetComponent<TextMesh>().text = human.GetComponent<Player>().money + " (" + human.GetComponent<Player>().GetMoneyIncome() + ")";
        zealText.GetComponent<TextMesh>().text = human.GetComponent<Player>().zeal + " (" + human.GetComponent<Player>().GetZealIncome() + ")";
    }
}
