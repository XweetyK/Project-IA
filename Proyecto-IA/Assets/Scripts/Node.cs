using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum State { Open, Close, Null };
    public List<Node> adjacents;
    public bool obstacle;
    public bool selected = false;
    public State nodeState = State.Null;
    private Vector3 _pos;
    public int _id;

    //Pathfinding
    public Node parent;

    public Node(Vector3 pos, int id){
        nodeState = State.Null;
        adjacents = new List<Node>();
        _pos = pos;
        _id = id;
    }
    public void AddAdjacents(Node node) {
        adjacents.Add(node);
    }

    public void Open(Node parent) {
        nodeState = State.Open;
        this.parent = parent;
    }
    public void Close() {
        nodeState = State.Close;
    }

    public void Null()
    {
        nodeState = State.Null;
    }
    public Vector3 Pos {
        get { return _pos; }
    }
}
