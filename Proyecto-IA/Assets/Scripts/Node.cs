using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private enum State { Open, Close, Null };
    public List<Node> adjacents;
    public bool obstacle;
    public bool selected = false;
    private State nodeState;
    public Vector3 pos;
    public int _id;

    public Node(Vector3 pos, int id){
        nodeState = State.Null;
        adjacents = new List<Node>();
        this.pos = pos;
        _id = id;
    }
    public void Adjacents(Node node) {
        adjacents.Add(node);
    }

    public void Open() {
        nodeState = State.Open;
    }
    public void Close() {
        nodeState = State.Close;
    }
}
