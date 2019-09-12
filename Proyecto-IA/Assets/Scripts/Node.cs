using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum state { Open, Close, Null };
    public List<Node> adjacents;
    public bool obstacle;
    public state nodeState;
    public Vector3 pos;
    public int _id;

    public Node(Vector3 pos, int id){
        this.pos = pos;
        _id = id;
    }
}
