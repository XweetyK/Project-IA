using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private enum FindMethod { BreadthFirst}
    [SerializeField] FindMethod currentMethod = FindMethod.BreadthFirst;
    private LinkedList<Node> openNodes;
    private List<Node> closedNodes;
    private List<Node> NodeList;
    private List<Node> NodePath;
    void Start()
    {
        openNodes = new LinkedList<Node>();
        closedNodes = new List<Node>();
        NodePath = new List<Node>();
    }

    List<Node> findPath(Node sourceNode,Node destNode){
        NodePath.Clear();
        NodeList = NodeSystem.Instance.nodeList;
        sourceNode.Open(null);
        openNodes.AddLast(sourceNode);
        Node currentNode = sourceNode;
        while(openNodes.Count > 0){
            if(currentNode == destNode){
                createPath(destNode);
                clear();
                return NodePath;
            }
            currentNode.Close();
            closedNodes.Add(currentNode);
            openNodes.Remove(currentNode);
            foreach (Node n in currentNode.adjacents){
                n.Open(currentNode);
                switch (currentMethod) {
                    case FindMethod.BreadthFirst:
                        openNodes.AddLast(n);
                        break;
                }
            }
            currentNode = openNodes.First.Value;
        }
        //clear();
        return null;
        
        //send to movement

    }
    void createPath(Node destNode){
        NodePath.Add(destNode);
        Node actualNode = destNode;
        while(actualNode.parent != null){
            NodePath.Add(actualNode.parent);
            actualNode = actualNode.parent;
        }
    }

    void clear(){
        foreach (Node n in openNodes){
            n.parent = null;
            n.Null();
        }
        foreach (Node n in closedNodes)
        {
            n.parent = null;
            n.Null();
        }
        openNodes.Clear();
        closedNodes.Clear();
    }



    
    

}
