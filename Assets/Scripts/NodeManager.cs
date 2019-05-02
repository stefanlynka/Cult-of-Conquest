using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<GameObject> Nodes = new List<GameObject>();

    float nodeDistance = 0.7f;
    float nodeDistanceBuffer = 0.2f;

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {
        CollectNodes();
        LabelNodes();
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void LabelNodes() {
        for (int i = 0; i < Nodes.Count; i++) {
            GameObject node = Nodes[i];
            SetNodeNeighbours(node);
        }
    }

    void CollectNodes() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject node = transform.GetChild(i).gameObject;
            if (node.GetComponent<Node>()) {
                Nodes.Add(node);
            }
        }
    }

    void SetNodeNeighbours(GameObject node) {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject otherNode = transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(node.transform.localPosition, otherNode.transform.localPosition);
            if (Mathf.Abs(distance-nodeDistance) < nodeDistanceBuffer ) {
                SituateNeighbours(node, otherNode);
            }
        }
    }

    void SituateNeighbours(GameObject node, GameObject otherNode) {
        node.GetComponent<Node>().Neighbours.Add(otherNode);
        if (otherNode.transform.localPosition.y >= node.transform.localPosition.y) {
            if (Mathf.Abs(node.transform.localPosition.x - otherNode.transform.localPosition.x) < nodeDistanceBuffer) {
                node.GetComponent<Node>().NeighbourUp = otherNode;
            }
            else if ((node.transform.localPosition.x + nodeDistanceBuffer) < otherNode.transform.localPosition.x) {
                node.GetComponent<Node>().NeighbourUpRight = otherNode;
            }
            else {
                node.GetComponent<Node>().NeighbourUpLeft = otherNode;
            }
        }
        else {
            if (Mathf.Abs(node.transform.localPosition.x - otherNode.transform.localPosition.x) < nodeDistanceBuffer) {
                node.GetComponent<Node>().NeighbourDown = otherNode;
            }
            else if ((node.transform.localPosition.x + nodeDistanceBuffer) < otherNode.transform.localPosition.x) {
                node.GetComponent<Node>().NeighbourDownRight = otherNode;
            }
            else {
                node.GetComponent<Node>().NeighbourDownLeft = otherNode;
            }
        }
    }

    public void UnhighlightNodes() {
        print("nodes getting unhighlighted");
        for (int i = 0; i  < Nodes.Count; i++) {
            GameObject node = Nodes[i];
            node.GetComponent<Node>().Unhighlight();
        }
    }

}
