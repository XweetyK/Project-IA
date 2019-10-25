using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : Singleton<Pathfinding> {
    private enum FindMethod { BreadthFirst, DepthFirst }
    [SerializeField] FindMethod currentMethod = FindMethod.BreadthFirst;
    private LinkedList<Node> openNodes;
    private List<Node> closedNodes;
    private List<Node> NodeList;
    private List<Node> NodePath;

    protected override void Initialize() {
        openNodes = new LinkedList<Node>();
        closedNodes = new List<Node>();
        NodePath = new List<Node>();
    }

    public List<Node> findPath(Node sourceNode, Node destNode) {
        clear();
        NodePath.Clear();
        NodeList = NodeSystem.Instance.nodeList;
        sourceNode.Open(null);
        openNodes.AddLast(sourceNode);
        Node currentNode = sourceNode;
        while (openNodes.Count > 0) {
            if (currentNode == destNode) {
                createPath(destNode);
                return NodePath;
            }
            currentNode.Close();
            closedNodes.Add(currentNode);
            openNodes.Remove(currentNode);
            foreach (Node n in currentNode.adjacents) {
                if (n.nodeState == Node.State.Null) {
                    n.Open(currentNode);
                    openNodes.AddLast(n);
                }
            }
            switch (currentMethod) {
                case FindMethod.BreadthFirst:
                    currentNode = openNodes.First.Value;
                    break;

                case FindMethod.DepthFirst:
                    currentNode = openNodes.Last.Value;
                    break;
            }
        }
        return null;
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
}
