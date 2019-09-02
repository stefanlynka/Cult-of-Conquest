using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour{

    int size;
    int timer = 120;
    float speed = 0.01f;

    // Start is called before the first frame update
    void Start(){
        //Setup(gameObject, "example text", 100, Color.green);
    }

    public void Setup(Vector3 position, string newText, int textSize, Color textColour, int timer, float speed) {
        transform.position = new Vector3(position.x, position.y, position.z - 10);
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<TextMesh>();
        GetComponent<TextMesh>().text = newText;
        GetComponent<TextMesh>().characterSize = 0.1f;
        GetComponent<TextMesh>().fontSize = textSize;
        GetComponent<TextMesh>().color = textColour;
        GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
        GetComponent<TextMesh>().alignment = TextAlignment.Center;
        this.timer = timer;
        this.speed = speed;
    }

    public void Setup(Vector3 position, string newText, int textSize, Color textColour) {
        transform.position = new Vector3(position.x, position.y, position.z - 10);
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<TextMesh>();
        GetComponent<TextMesh>().text = newText;
        GetComponent<TextMesh>().characterSize = 0.1f;
        GetComponent<TextMesh>().fontSize = textSize;
        GetComponent<TextMesh>().color = textColour;
        GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
        GetComponent<TextMesh>().alignment = TextAlignment.Center;
    }

    // Update is called once per frame
    void Update(){
        transform.Translate(new Vector3(0, speed, 0));
        timer--;
        Color newColour = GetComponent<TextMesh>().color;
        newColour.a = timer / 60f;
        GetComponent<TextMesh>().color = newColour;
        if (timer <= 0) {
            print("done");
            Destroy(gameObject);
        }
    }
}
