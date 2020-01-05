using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimator : MonoBehaviour{

    Vector3 target;
    bool atTarget = true;
    bool attacking = false;
    float moveSpeed = 0.05f;
    int waitingTime = 0;
    GameObject targetNode = null;

    // Start is called before the first frame update
    void Start(){
        SetTargetPosition(transform.position);
        atTarget = true;
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, target)>0.01) MoveToTarget();
    }
    public void SetTargetPosition(Vector3 targetPos) {
        target = targetPos;
        attacking = false;
        atTarget = false;
    }
    public void SetTargetMove(Vector3 targetPos, GameObject destinationNode) {
        target = targetPos;
        attacking = false;
        atTarget = false;
        targetNode = destinationNode;
    }
    public void SetTargetAttack(Vector3 targetPos, GameObject destinationNode) {
        target = targetPos;
        attacking = true;
        atTarget = false;
        targetNode = destinationNode;
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
                if (attacking) {
                    print("Arrived at battlefield "+targetNode);
                    GetComponent<Army>().owner.GetComponent<Player>().PrepBattle(gameObject, targetNode);
                    GameObject.Find("/Battle Menu").GetComponent<BattleMenu>().EnterMenu();
                }
                if (!attacking) {
                    print(gameObject.name + " at Destination: " + target);
                    GetComponent<Army>().OccupyNode(targetNode);
                    Army.readyToMove = true;
                    Army.armyAttacking = false;
                    //print("at destination, ready to execute");
                    if (GetComponent<Army>().owner.GetComponent<AI>()) GetComponent<Army>().owner.GetComponent<AI>().readyToExecute = true;
                }
            }
        }
    }
}
