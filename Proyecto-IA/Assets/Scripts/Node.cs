using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum state { Open, Close, Null };
    public List<Node> adjacents;
    public bool obstacle;
    public bool selected = false;
    public state nodeState;
    public Vector3 pos;
    public int _id;

    public Node(Vector3 pos, int id){
        adjacents = new List<Node>();
        this.pos = pos;
        _id = id;
    }
    public void Adjacents(Node node) {
        adjacents.Add(node);
    }
}
