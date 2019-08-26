using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour{

    public Upgrade upgrade;
    GameObject human, indicators;

    // Start is called before the first frame update
    void Start(){
        human = Player.human;
        indicators = Tools.GetChildNamed(gameObject, "Upgrade Indicators");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        if (Player.menuOpen == 1) {
            if (human.GetComponent<Player>().BuyUpgrade(upgrade)) {
                UpdateIndicator(upgrade);
                //transform.parent.transform.parent.GetComponent<UpgradeMenu>().ExitMenu();
            }
        }
    }
    void UpdateIndicator(Upgrade upgrade) {
        for (int i = 0; i < upgrade.currentLevel; i++) {
            GameObject indicator = Tools.GetChildNamed(indicators, "Unupgraded Indicator " + i.ToString());
            indicator.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/Upgraded Indicator");
        }
    }
}
