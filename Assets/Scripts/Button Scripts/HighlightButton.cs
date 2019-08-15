using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightButton : MonoBehaviour {

    bool mouseOver = false;
    Color defaultColour;

    // Start is called before the first frame update
    void Start(){
        GameObject target = Tools.GetChildNameContains(gameObject, "Sprite");
        if (target != null) defaultColour = target.GetComponent<SpriteRenderer>().color;
        if (GetComponent<SpriteRenderer>()) defaultColour = GetComponent<SpriteRenderer>().color;


    }

    // Update is called once per frame
    void Update() {
        UpdateColour();
    }

    private void OnMouseOver() {
        mouseOver = true;
    }
    private void UpdateColour() {
        Color newColour;
        if (mouseOver) {newColour = Color.gray;}
        else { newColour = defaultColour; }
        GameObject target = Tools.GetChildNameContains(gameObject, "Sprite");
        if (GetComponent<SpriteRenderer>()) target = gameObject;
        if (target != null) SetColour(target, newColour);
        mouseOver = false;
    }
    private void SetColour(GameObject target, Color colour) {
        target.GetComponent<SpriteRenderer>().color = colour;
    }
}
