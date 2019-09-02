using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static List<GameObject> nodes = new List<GameObject>();
    public static GameObject highlightFog;

    float nodeDistance = 0.7f;
    float nodeDistanceBuffer = 0.2f;

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Startup() {
        CollectNodes();
        LabelNodes();
        highlightFog = GameObject.Find("/Highlight");
        highlightFog.transform.position = new Vector3(0, 0, 10);
        highlightFog.SetActive(false);
    }

    void LabelNodes() {
        for (int i = 0; i < nodes.Count; i++) {
            GameObject node = nodes[i];
            SetNodeNeighbours(node);
            node.GetComponent<Node>().UpdateSprite();
        }
    }

    void CollectNodes() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject node = transform.GetChild(i).gameObject;
            if (node.GetComponent<Node>()) {
                nodes.Add(node);
            }
        }
        print("nodes are collected");
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

    public void HighlightAttackableNodes(GameObject node, int distance, Faction faction) {
        highlightFog.SetActive(true);

        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            if (neighbour.GetComponent<Node>().occupiable) {
                neighbour.GetComponent<Node>().Highlight();
                if (distance > 1 && neighbour.GetComponent<Node>().faction == faction) {
                    HighlightAttackableNodes(neighbour, distance - 1, faction);
                }
            }
        }
    }

    public void HighlightNodes(GameObject node, int distance) {
        highlightFog.SetActive(true);

        print("Highlighting Nodes");
        List<GameObject> neighbours = node.GetComponent<Node>().neighbours;
        for (int i = 0; i < neighbours.Count; i++) {
            GameObject neighbour = neighbours[i];
            neighbour.GetComponent<Node>().Highlight();
            if (distance > 1) {
                HighlightNodes(neighbour, distance - 1);
            }
        }
    }

    public void UnhighlightNodes() {
        highlightFog.SetActive(false);

        for (int i = 0; i  < nodes.Count; i++) {
            GameObject node = nodes[i];
            node.GetComponent<Node>().Unhighlight();
        }
    }
    public GameObject GetRandomNode() {
        int randInt = Random.Range(0, nodes.Count);
        GameObject node = nodes[randInt];
        return node;
    }

    public void SetNodesToHidden() {
        for(int i = 0; i < nodes.Count; i++) {
            GameObject node = nodes[i];
            node.GetComponent<Node>().hidden = true;
        }
    }
    public void HideStillHiddenNodes() {
        for (int i = 0; i < nodes.Count; i++) {
            GameObject node = nodes[i];
            node.GetComponent<Node>().SetFog();
        }
    }

}
