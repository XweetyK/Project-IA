using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    private enum FindMethod { BreadthFirst, DepthFirst }
    [SerializeField] FindMethod currentMethod = FindMethod.BreadthFirst;
    private LinkedList<Node> openNodes;
    private List<Node> closedNodes;
    private List<Node> NodeList;
    private List<Node> NodePath;
    private bool noPath;
    void Start() {
        openNodes = new LinkedList<Node>();
        closedNodes = new List<Node>();
        NodePath = new List<Node>();
        noPath = false;
    }

    public List<Node> FindPath(Node sourceNode, Node destNode) {

        clear();
        noPath = false;
        NodePath.Clear();
        NodeList = NodeSystem.Instance.nodeList;
        foreach (Node n in NodeList) {
            n.selected = false;
        }
        if (sourceNode != null) {
            sourceNode.selected = true;
        }
        if (destNode != null) {
            destNode.selected = true;
        }
        sourceNode.Open(null);
        openNodes.AddLast(sourceNode);
        Node currentNode = sourceNode;
        while (openNodes.Count > 0 && noPath==false) {
            if (currentNode == destNode) {
                createPath(destNode);
                return NodePath;
            }
            currentNode.Close();
            closedNodes.Add(currentNode);
            openNodes.Remove(currentNode);
            foreach (Node n in currentNode.adjacents) {
                if (n.nodeState == Node.State.Null && n.obstacle==false) {
                    n.Open(currentNode);
                    openNodes.AddLast(n);
                }
            }
            switch (currentMethod) {
                case FindMethod.BreadthFirst:
                    if (openNodes.First != null) {
                        currentNode = openNodes.First.Value;
                    } else { noPath = true; }
                    break;

                case FindMethod.DepthFirst:
                    if (openNodes.Last != null) {
                        currentNode = openNodes.Last.Value;
                    } else { noPath = true; }
                    break;
            }
        }
        noPath = true;
        Debug.Log("No path!");
        return null;

        //send to movement

    }
    void createPath(Node destNode) {
        NodePath.Add(destNode);
        Node actualNode = destNode;
        while (actualNode.parent != null) {
            NodePath.Add(actualNode.parent);
            actualNode = actualNode.parent;
        }
    }

    void clear() {
        foreach (Node n in openNodes) {
            n.parent = null;
            n.Null();
        }
        foreach (Node n in closedNodes) {
            n.parent = null;
            n.Null();
        }
        openNodes.Clear();
        closedNodes.Clear();
    }

    public Node GetNodeByCoord(Vector3 pos) {
        Node node=null;
        float dist = 5;
        foreach (Node n in NodeSystem.Instance.nodeList) {
            if (Vector3.Distance(n.Pos, pos) < dist) {
                node = n;
                dist = Vector3.Distance(n.Pos, pos);
            }
        }
        if (node != null) { return node; }
        return null;
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) {
            if (NodeSystem.Instance.nodeList.Count != 0) {
                foreach (Node n in NodeSystem.Instance.nodeList) {
                    if (n.selected) {
                        Gizmos.color = Color.green;
                    } else if (n.nodeState == Node.State.Open) {
                        Gizmos.color = Color.yellow;
                    } else if (n.nodeState == Node.State.Close) {
                        Gizmos.color = Color.red;
                    } else { Gizmos.color = Color.black; }
                    if (n.obstacle) {
                        Gizmos.color = Color.magenta;
                    }
                    Gizmos.DrawWireSphere(n.Pos, 0.5f);
                }
            }
            if (NodePath != null) {
                foreach (Node n in NodePath) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(n.Pos, 0.5f);
                }
            }
        }
    }



}
