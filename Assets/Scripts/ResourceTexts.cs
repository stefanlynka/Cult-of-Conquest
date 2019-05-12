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
    void Update(){
        moneyText.GetComponent<TextMesh>().text = Player.money.ToString();
        zealText.GetComponent<TextMesh>().text = Player.zeal.ToString();
    }
}
