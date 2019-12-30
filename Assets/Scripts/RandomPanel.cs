using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPanel : MonoBehaviour{

    public List<GameObject> targets = new List<GameObject>();
    public GameObject attackingArmy, infoText;
    float randomTargetBonus = 0.035f;

    // Start is called before the first frame update
    void Start(){
        GameObject info = Tools.GetChildNamed(gameObject, "Random Info");
        infoText = Tools.GetChildNamed(info, "Random Info Text");

    }

    // Update is called once per frame
    void Update(){
        attackingArmy = Player.selectedArmy;
        if (attackingArmy == null) {
            foreach (GameObject target in targets) {
                target.GetComponent<Node>().UpdateSprite();
            }
            targets.Clear();
            UpdateText();
        }
    }
    public void Setup() {
        if (Player.human.GetComponent<Player>().faction == Faction.Carnot) gameObject.GetComponent<Panner>().SetTarget(new Vector3(6.9f, 0, -5));
        else gameObject.SetActive(false);
    }

    public void AddTarget(GameObject node) {
        if (!targets.Contains(node)) {
            targets.Add(node);
            node.GetComponent<SpriteRenderer>().color = Color.grey;
            print("target added");
        }
        else {
            targets.Remove(node);
            node.GetComponent<SpriteRenderer>().color = node.GetComponent<Node>().defaultColour;
            print("target removed");
        }
        UpdateText();
    }
    void UpdateText() {
        float bonusPerTarget = randomTargetBonus;
        if (Player.human) {
            if (Player.human.GetComponent<Player>().upgrades.ContainsKey("Assaulting Anarchy")) bonusPerTarget += randomTargetBonus * Player.human.GetComponent<Player>().upgrades["Assaulting Anarchy"].currentLevel;
            float modifier = targets.Count * bonusPerTarget;
            infoText.GetComponent<TextMesh>().text = "Nodes Selected: " + targets.Count + "\nCombat Bonus: " + (modifier*100).ToString() + "%";
        }
    }

    public void Attack() {
        int randInt = Random.Range(0, targets.Count);
        GameObject target = targets[randInt];

        float bonusPerTarget = randomTargetBonus;
        if (Player.human.GetComponent<Player>().upgrades.ContainsKey("Assaulting Anarchy")) bonusPerTarget += randomTargetBonus * Player.human.GetComponent<Player>().upgrades["Assaulting Anarchy"].currentLevel;
        float modifier = targets.Count * bonusPerTarget;
        Player.selectedArmy.GetComponent<Army>().AddToDamageMod(modifier);
        //print(armyMod: )
        Player.human.GetComponent<Player>().attackNode(Player.selectedArmy, target);
    }

}
