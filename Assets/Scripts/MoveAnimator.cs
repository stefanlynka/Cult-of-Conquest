using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimator : MonoBehaviour{

    Vector3 target;
    bool atTarget;
    bool attacking = false;
    float moveSpeed = 0.05f;
    int waitingTime = 0;

    // Start is called before the first frame update
    void Start(){
        SetTarget(transform.position, false);
    }

    // Update is called once per frame
    void Update() {
        MoveToTarget();
    }
    public void SetTarget(Vector3 targetPos, bool isAttacking) {
        target = targetPos;
        attacking = isAttacking;
    }
    void MoveToTarget() {
        if (Player.menuOpen != 0) {
            waitingTime = 60;
        }
        else waitingTime--;
        if (Player.menuOpen == 0 && waitingTime <= 0) {
            Vector2 currentPos = transform.localPosition;
            Vector2 targetPos = target;
            if (Vector2.Distance(currentPos, targetPos) > 0.001f) {
                atTarget = false;
                Vector2 distanceToTarget = targetPos - currentPos;
                Vector2 direction = distanceToTarget.normalized;
                direction *= moveSpeed;
                if (direction.magnitude > distanceToTarget.magnitude) direction = distanceToTarget;
                Vector3 offset = direction;
                offset.z = 0;
                transform.localPosition += offset;
            }
            else if (!atTarget) {
                atTarget = true;
                if (attacking) GameObject.Find("/Battle Menu").GetComponent<BattleMenu>().EnterMenu();
                if (!attacking) {
                    Army.readyToMove = true;
                    //print("at destination, ready to execute");
                    if (GetComponent<Army>().owner.GetComponent<AI>()) GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
                }
            }
        }
    }
}
