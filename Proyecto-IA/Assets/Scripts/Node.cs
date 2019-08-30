﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum state { Open, Close, Null };
    public List<Node> adjacents;
    public bool obstacle;
    public state nodeState;
    public Vector3 pos;

    public Node(Vector3 pos){
        this.pos = pos;
    }

}
