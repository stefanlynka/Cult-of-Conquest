using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static List<GameObject> nodes = new List<GameObject>();

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
        for (int i = 0; i < nodes.Count; i++) {
            GameObject node = nodes[i];
            SetNodeNeighbours(node);
        }
    }

    void CollectNodes() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject node = transform.GetChild(i).gameObject;
            if (node.GetComponent<Node>()) {
                nodes.Add(node);
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
        node.GetComponent<Node>().neighbours.Add(otherNode);
        if (otherNode.transform.localPosition.y >= node.transform.localPosition.y) {
            if (Mathf.Abs(node.transform.localPosition.x - otherNode.transform.localPosition.x) < nodeDistanceBuffer) {
                node.GetComponent<Node>().neighbourUp = otherNode;
            }
            else if ((node.transform.localPosition.x + nodeDistanceBuffer) < otherNode.transform.localPosition.x) {
                node.GetComponent<Node>().neighbourUpRight = otherNode;
            }
            else {
                node.GetComponent<Node>().neighbourUpLeft = otherNode;
            }
        }
        else {
            if (Mathf.Abs(node.transform.localPosition.x - otherNode.transform.localPosition.x) < nodeDistanceBuffer) {
                node.GetComponent<Node>().neighbourDown = otherNode;
            }
            else if ((node.transform.localPosition.x + nodeDistanceBuffer) < otherNode.transform.localPosition.x) {
                node.GetComponent<Node>().neighbourDownRight = otherNode;
            }
            else {
                node.GetComponent<Node>().neighbourDownLeft = otherNode;
            }
        }
    }

    public void UnhighlightNodes() {
        for (int i = 0; i  < nodes.Count; i++) {
            GameObject node = nodes[i];
            node.GetComponent<Node>().Unhighlight();
        }
    }

}
