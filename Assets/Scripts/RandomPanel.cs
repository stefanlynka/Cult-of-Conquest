using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPanel : MonoBehaviour{

    public List<GameObject> targets = new List<GameObject>();
    public GameObject attackingArmy, infoText;

    // Start is called before the first frame update
    void Start(){
        GameObject info = Tools.GetChildNamed(gameObject, "Random Info");
        infoText = Tools.GetChildNamed(info, "Random Info Text");

    }

    // Update is called once per frame
    void Update(){
        attackingArmy = Player.selectedArmy;
        if (attackingArmy == null) {
            targets.Clear();
            UpdateText();
        }
    }
    public void Setup() {
        if (Player.human.GetComponent<Player>().faction != Faction.Carnot) gameObject.GetComponent<Panner>().SetTarget(new Vector3(20, 0, 0));
    }

    public void AddTarget(GameObject node) {
        if (!targets.Contains(node)) {
            targets.Add(node);
            print("target added");
        }
        else {
            targets.Remove(node);
            print("target removed");
        }
        UpdateText();
    }
    void UpdateText() {
        infoText.GetComponent<TextMesh>().text = "Nodes Selected: " + targets.Count + "\nCombat Bonus: " + (targets.Count * 5).ToString() + "%";
    }

    public void Attack() {
        int randInt = Random.Range(0, targets.Count);
        GameObject target = targets[randInt];
        Player.human.GetComponent<Player>().attackNode(Player.selectedArmy, target);
    }

}
