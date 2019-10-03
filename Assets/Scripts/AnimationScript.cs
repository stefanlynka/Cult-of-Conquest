using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour{

    int timer = 120;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        timer--;
        if (timer <= 0) Destroy(gameObject);
    }
    public void Setup(Vector3 position, int newTimer) {
        transform.position = position;
        timer = newTimer;
    }
}
