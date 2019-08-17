using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackButton : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnMouseDown() {
        transform.parent.GetComponent<RandomPanel>().Attack();
    }
}
