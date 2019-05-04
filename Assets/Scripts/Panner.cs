using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panner : MonoBehaviour
{
    Vector3 target;
    int panSpeed = 20;

    // Start is called before the first frame update
    void Start(){
        target = transform.position;
    }

    // Update is called once per frame
    void Update(){
        MoveToTarget();
    }
    public void SetTarget(Vector3 targetPos) {
        target = targetPos;
    }
    void MoveToTarget() {
        if (Vector3.Distance(transform.localPosition, target) > 0.01f) {
            transform.localPosition += (target - transform.localPosition) / panSpeed;
        }
    }
}
